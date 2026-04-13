using bowling_tournament_MVCPRoject.Domain.Dtos.Requests;
using bowling_tournament_MVCPRoject.Domain.Entities;
using bowling_tournament_MVCPRoject.Domain.Services;
using bowling_tournament_MVCPRoject.UI.Queries;
using bowling_tournament_MVCPRoject.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bowling_tournament_MVCPRoject.UI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ITeamManagerService _teamService;
        private readonly ITournamentService _tournamentService;
        private readonly ITeamReadModelGateway _teamGateway;
        private readonly ITournamentReadModelGateway _tournamentGateway;
        private readonly IRegistrationReadModelGateway _registrationGateway;

        public AdminController(
            ITeamManagerService teamService,
            ITournamentService tournamentService,
            ITeamReadModelGateway teamGateway,
            ITournamentReadModelGateway tournamentGateway,
            IRegistrationReadModelGateway registrationGateway)
        {
            _teamService = teamService;
            _tournamentService = tournamentService;
            _teamGateway = teamGateway;
            _tournamentGateway = tournamentGateway;
            _registrationGateway = registrationGateway;
        }

        private bool IsAdmin() => User.HasClaim("IsAdmin", "true");

        // TEAM REGISTRATION
        [HttpGet]
        public async Task<IActionResult> TeamRegistrationAdmin(string? filterBy, int? division, RegistrationStatus? paid, string? sortBy, string? order)
        //Note to Nick when you see this-- basically I've converted this former team list into a registration list.
        //Registrations are basically paid through this whereas the new team list handles creating new registrations
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");

            var registrations = await _teamGateway.GetAllTeamRegistrations();

            if (filterBy == "Division" && division.HasValue)
                registrations = registrations.Where(t => t.team.teamDivision == division.Value).ToList();

            if (filterBy == "PaymentStatus" && paid.HasValue)
                registrations = registrations.Where(t => t.registrationStatus == (RegistrationStatus)paid).ToList();

            bool descending = order == "desc";
            registrations = sortBy switch
            {
                "Name" => descending ? registrations.OrderByDescending(t => t.team.teamName).ToList() : registrations.OrderBy(t => t.team.teamName).ToList(),
                "PaymentStatus" => descending ? registrations.OrderByDescending(t => t.registrationStatus).ToList() : registrations.OrderBy(t => t.registrationStatus).ToList(),
                "PaymentDate" => descending ? registrations.OrderByDescending(t => t.statusDate).ToList() : registrations.OrderBy(t => t.statusDate).ToList(),
                _ => registrations
            };

            return View(registrations);
        }

        [HttpGet]
        public async Task<IActionResult> TeamListAdmin(string? filterBy, int? division, string? sortBy, string? order)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");

            var teams = await _teamGateway.GetAllAsync();

            if (filterBy == "Division" && division.HasValue)
                teams = teams.Where(t => t.teamDivision == division.Value).ToList();

            bool descending = order == "desc";
            teams = sortBy switch
            {
                "Name" => descending ? teams.OrderByDescending(t => t.teamName).ToList() : teams.OrderBy(t => t.teamName).ToList(),
                _ => teams
            };

            return View(teams);
        }



        // TEAM DETAILS
        [HttpGet]
        public async Task<IActionResult> DetailsAdmin(int id)
        {
            var team = await _teamGateway.GetByIdAsync(id);
            if (team == null) return NotFound();
            return View("/Ui/Views/Home/Details.cshtml", team);
        }

        // EDIT TEAM
        [HttpGet]
        public async Task<IActionResult> EditTeam(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            Console.WriteLine("DEBUG: ID AT VIEW LEVEL IS " + id);
            var team = await _teamGateway.GetByIdAsync(id);
            if (team == null) return NotFound();

            var vm = new TeamEditVm
            {
                TeamId = team.id,
                TeamName = team.teamName,
                DivisionId = team.teamDivision,
                DivisionOptions = await _teamGateway.GetDivisionOptionsAsync(),
                Players = team.Players
            };
            return View("Edit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditTeam(TeamEditVm vm)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");

            vm.DivisionOptions = await _teamGateway.GetDivisionOptionsAsync();
            if (!ModelState.IsValid) return View("Edit", vm);

            var request = new TeamRequest(vm.TeamId ?? 0, vm.TeamName ?? "", vm.DivisionId);
            var result = _teamService.tryUpdateTeam(request);

            if (!result.success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View("Edit", vm);
            }

            return RedirectToAction("EditTeam", new { id = vm.TeamId });
        }

        // DELETE TEAM
        [HttpGet]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var team = await _teamGateway.GetByIdAsync(id);
            if (team == null) return NotFound();
            return View("Delete", team);
        }
        [HttpPost]
        public IActionResult DeleteTeamConfirmed(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var result = _teamService.tryDeleteTeam(new TeamRequest(id, "", 0));
            if (!result.success) return NotFound();
            TempData["Message"] = $"Team {id} deleted.";
            return RedirectToAction("TeamListAdmin");
        }

        //DELETE REGISTRATION
        //Put here for now as is as deleting registrations not *strictly* necessary yet.
        [HttpGet]
        public async Task<IActionResult> DeleteRegistration(int registrationId, int teamId, int tournamentId)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var registration = await _registrationGateway.GetByIdAsync(registrationId);
            if (registration == null) return NotFound();

            var result = _teamService.tryCancelRegistration(new RegisterTeamRequest(registrationId, teamId, tournamentId));
            if (!result.success) TempData["Message"] = "Something went wrong during cancellation, please try again";
            else TempData["Message"] = $"Team unregistered from tournament ID {tournamentId}";
            return RedirectToAction("TeamRegistrationAdmin");
        }

        [HttpPost]
        public IActionResult DeleteRegistrationConfirmed(int registrationId)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");



            return RedirectToAction("TeamRegistrationAdmin");
        }

        // EDIT PLAYER
        [HttpGet]
        public async Task<IActionResult> EditPlayer(int playerId, int teamId)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var team = await _teamGateway.GetByIdAsync(teamId);
            if (team == null) return NotFound();

            var player = team.Players.FirstOrDefault(p => p.PlayerId == playerId);
            if (player == null) return NotFound();

            var vm = new PlayerEditVm
            {
                PlayerId = player.PlayerId,
                TeamId = teamId,
                PlayerName = player.Name ?? "",
                City = player.City ?? "",
                Province = player.Province ?? "",
                Email = player.Email ?? "",
                Phone = player.Phone ?? ""
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EditPlayer(PlayerEditVm vm)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            if (!ModelState.IsValid) return View(vm);

            var request = new PlayerRequest(
                vm.PlayerId ?? 0,
                vm.TeamId,
                vm.PlayerName,
                vm.Email,
                vm.City,
                vm.Province,
                vm.Phone
            );

            var result = _teamService.tryUpdatePlayer(request);
            if (!result.success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(vm);
            }

            return RedirectToAction("EditTeam", new { id = vm.TeamId });
        }

        // REMOVE PLAYER
        [HttpPost]
        public IActionResult RemovePlayer(int id, int teamId)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var result = _teamService.tryDeletePlayer(new PlayerRequest(id, teamId, "", "", "", "", ""));
            TempData["Message"] = result.success ? "Player removed." : result.Errors.FirstOrDefault();
            return RedirectToAction("EditTeam", new { id = teamId });
        }

        // MARK PAID
        [HttpGet]
        public IActionResult MarkPaid(int teamId)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var request = new RegisterTeamRequest(0, teamId, 0);
            var result = _teamService.tryMarkPaid(request);
            TempData["Message"] = result.success ? "Team marked as paid." : result.Errors.FirstOrDefault();
            return RedirectToAction("TeamListAdmin");
        }

        // SUMMARY
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var teams = await _teamGateway.GetAllAsync();
            const decimal fee = 200.00m;

            var summary = teams
                .GroupBy(t => t.divisionName ?? "Unknown")
                .Select(g => new SummaryVM
                {
                    DivisionName = g.Key,
                    Teams = g.Count(),
                    PayingTeams = g.Count(t => t.isPaid),
                    TotalFees = g.Count(t => t.isPaid) * fee
                }).ToList();

            summary.Add(new SummaryVM
            {
                DivisionName = "Overall",
                Teams = teams.Count,
                PayingTeams = teams.Count(t => t.isPaid),
                TotalFees = teams.Count(t => t.isPaid) * fee
            });

            return View(summary);
        }

        // CREATE TOURNAMENT
        [HttpGet]
        public IActionResult CreateTournament()
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            return View(new TournamentCreateVm());
        }

        [HttpPost]
        public IActionResult CreateTournament(TournamentCreateVm vm)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            if (!ModelState.IsValid) return View(vm);

            var request = new TournamentRequest(0, vm.Name ?? "",
                vm.WatcherCapacity,
                vm.DateOfGame,
                vm.Location ?? "",
                vm.TeamCapacity,
                vm.RegistrationOpen,
                vm.MenCapacity,
                vm.WomenCapacity,
                vm.MixedCapacity,
                vm.YouthCapacity,
                vm.SeniorCapacity);
            var result = _tournamentService.tryRegisterTournament(request);

            if (!result.success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(vm);
            }

            TempData["Message"] = "Tournament created successfully.";
            return RedirectToAction("Index", "Home");
        }

        // EDIT TOURNAMENT
        [HttpGet]
        public async Task<IActionResult> EditTournament(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var tournament = await _tournamentGateway.GetByIdAsync(id);
            if (tournament == null) return NotFound();

            var vm = new TournamentEditVm
            {
                Id = tournament.id,
                Name = tournament.tournamentName ?? "",
                Location = tournament.location ?? "",
                DateOfGame = tournament.tournamentDate,
                TeamCapacity = tournament.teamCapacity,
                WatcherCapacity = tournament.watcherCapacity,
                RegistrationOpen = tournament.registrationOpen,
                MenCapacity = tournament.menCapacity,
                WomenCapacity = tournament.womenCapacity,
                MixedCapacity = tournament.mixedCapacity,
                YouthCapacity = tournament.youthCapacity,
                SeniorCapacity = tournament.seniorCapacity
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EditTournament(TournamentEditVm vm)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            if (!ModelState.IsValid) return View(vm);

            var request = new TournamentRequest(vm.Id,
                vm.Name ?? "",
                vm.WatcherCapacity,
                vm.DateOfGame,
                vm.Location ?? "",
                vm.TeamCapacity,
                vm.RegistrationOpen,
                vm.MenCapacity,
                vm.WomenCapacity,
                vm.MixedCapacity,
                vm.YouthCapacity,
                vm.SeniorCapacity);
            var result = _tournamentService.tryUpdateTournament(request);

            if (!result.success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(vm);
            }

            TempData["Message"] = "Tournament updated successfully.";
            return RedirectToAction("EditTournament", new { id = vm.Id });
        }

        // REGISTER TEAM FOR TOURNAMENT
        [HttpGet]
        public async Task<IActionResult> RegisterTeam(int teamId)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var tournaments = await _tournamentGateway.GetAllAsync();
            var vm = new RegisterTeamVm
            {
                TeamId = teamId,
                TournamentOptions = tournaments.Select(t => new SelectListItem
                {
                    Value = t.id.ToString(),
                    Text = t.tournamentName
                })
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult RegisterTeam(RegisterTeamVm vm)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var request = new RegisterTeamRequest(0, vm.TeamId, vm.TournamentId);
            var result = _teamService.tryRegisterTeam(request);
            if (!result.success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(vm);
            }
            TempData["Message"] = "Team registered for tournament successfully.";
            return RedirectToAction("TeamRegistrationAdmin");
        }

        [HttpGet]
        public IActionResult AddPlayer(int teamId)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var vm = new PlayerEditVm { TeamId = teamId };
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddPlayer(PlayerEditVm vm)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            if (!ModelState.IsValid) return View(vm);

            var request = new PlayerRequest(
                0, vm.TeamId, vm.PlayerName, vm.Email, vm.City, vm.Province, vm.Phone
            );
            var result = _teamService.tryAddPlayer(request);
            if (!result.success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
                return View(vm);
            }
            return RedirectToAction("EditTeam", new { id = vm.TeamId });
        }

        [HttpGet]
        public async Task<IActionResult> ViewWaitlist(int tournamentId)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");

            var waitlistRegistrations = await _registrationGateway.GetWaitlistedByTournamentAsync(tournamentId);

            if (waitlistRegistrations == null || waitlistRegistrations.Count == 0)
            {
                TempData["Message"] = "No teams are currently waitlisted for this tournament.";
                return RedirectToAction("ViewTournaments", "Home");
            }

            ViewData["TournamentId"] = tournamentId;
            if (waitlistRegistrations.Count > 0)
            {
                ViewData["TournamentName"] = waitlistRegistrations[0].tournament.tournamentName;
            }

            return View(waitlistRegistrations);
        }
    }
}