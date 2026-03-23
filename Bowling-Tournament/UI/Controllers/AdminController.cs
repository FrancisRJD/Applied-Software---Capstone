using bowling_tournament_MVCPRoject.Domain.Dtos.Requests;
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

        public AdminController(
            ITeamManagerService teamService,
            ITournamentService tournamentService,
            ITeamReadModelGateway teamGateway,
            ITournamentReadModelGateway tournamentGateway)
        {
            _teamService = teamService;
            _tournamentService = tournamentService;
            _teamGateway = teamGateway;
            _tournamentGateway = tournamentGateway;
        }

        private bool IsAdmin() => User.HasClaim("IsAdmin", "true");

        // TEAM LIST
        [HttpGet]
        public async Task<IActionResult> TeamListAdmin(string? filterBy, int? division, bool? paid, string? sortBy, string? order)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");

            var teams = await _teamGateway.GetAllWithStatusAsync();

            if (filterBy == "Division" && division.HasValue)
                teams = teams.Where(t => t.teamDivision == division.Value).ToList();

            if (filterBy == "PaymentStatus" && paid.HasValue)
                teams = teams.Where(t => t.IsPaid == paid.Value).ToList();

            bool descending = order == "desc";
            teams = sortBy switch
            {
                "Name" => descending ? teams.OrderByDescending(t => t.teamName).ToList() : teams.OrderBy(t => t.teamName).ToList(),
                "PaymentStatus" => descending ? teams.OrderByDescending(t => t.IsPaid).ToList() : teams.OrderBy(t => t.IsPaid).ToList(),
                "PaymentDate" => descending ? teams.OrderByDescending(t => t.DatePaid).ToList() : teams.OrderBy(t => t.DatePaid).ToList(),
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

            var request = new TeamRequest(vm.TeamId ?? 0, vm.TeamName, vm.DivisionId);
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
            var result = _teamService.tryMarkRegistrationPaid(request);
            TempData["Message"] = result.success ? "Team marked as paid." : result.Errors.FirstOrDefault();
            return RedirectToAction("TeamListAdmin");
        }

        // SUMMARY
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            var teams = await _teamGateway.GetAllWithStatusAsync();
            const decimal fee = 200.00m;

            var summary = teams
                .GroupBy(t => t.divisionName ?? "Unknown")
                .Select(g => new SummaryVM
                {
                    DivisionName = g.Key,
                    Teams = g.Count(),
                    PayingTeams = g.Count(t => t.IsPaid),
                    TotalFees = g.Count(t => t.IsPaid) * fee
                }).ToList();

            summary.Add(new SummaryVM
            {
                DivisionName = "Overall",
                Teams = teams.Count,
                PayingTeams = teams.Count(t => t.IsPaid),
                TotalFees = teams.Count(t => t.IsPaid) * fee
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

            var request = new TournamentRequest(0, vm.Name, vm.WatcherCapacity, vm.DateOfGame, vm.Location, vm.TeamCapacity, vm.RegistrationOpen);
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
                RegistrationOpen = tournament.registrationOpen
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EditTournament(TournamentEditVm vm)
        {
            if (!IsAdmin()) return RedirectToAction("Denied", "Auth");
            if (!ModelState.IsValid) return View(vm);

            var request = new TournamentRequest(vm.Id, vm.Name, vm.WatcherCapacity, vm.DateOfGame, vm.Location, vm.TeamCapacity, vm.RegistrationOpen);
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
            return RedirectToAction("TeamListAdmin");
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
    }
}