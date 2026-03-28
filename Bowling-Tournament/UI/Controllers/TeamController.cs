using bowling_tournament_MVCPRoject.Domain.Dtos.Requests;
using bowling_tournament_MVCPRoject.Domain.Services;
using bowling_tournament_MVCPRoject.UI.Queries;
using bowling_tournament_MVCPRoject.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace bowling_tournament_MVCPRoject.UI.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamManagerService _teamService;
        private readonly ITeamReadModelGateway _teamGateway;
        private readonly IRegistrationReadModelGateway _registrationGateway;

        public TeamController(ITeamManagerService teamService, ITeamReadModelGateway teamGateway, IRegistrationReadModelGateway registrationGateway)
        {
            _teamService = teamService;
            _teamGateway = teamGateway;
            _registrationGateway = registrationGateway;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var vm = new TeamRegisterVm
            {
                DivisionOptions = await _teamGateway.GetDivisionOptionsAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Register(TeamRegisterVm vm)
        {
            vm.DivisionOptions = await _teamGateway.GetDivisionOptionsAsync();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (vm.Players.Count != 4)
            {
                ModelState.AddModelError("", "You need exactly four players.");
                return View(vm);
            }

            var teamRequest = new TeamRequest(0, vm.TeamName ?? "", vm.DivisionId);
            var teamResult = _teamService.tryCreateTeam(teamRequest);

            if (!teamResult.success)
            {
                foreach (var error in teamResult.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(vm);
            }

            foreach (var p in vm.Players)
            {
                var playerRequest = new PlayerRequest(
                    0,
                    teamResult.team.TeamId,
                    p.PlayerName ?? "",
                    p.Email ?? "",
                    p.City ?? "",
                    p.Province ?? "",
                    p.Phone ?? ""
                );

                var playerResult = _teamService.tryAddPlayer(playerRequest);

                if (!playerResult.success)
                {
                    foreach (var error in playerResult.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View(vm);
                }
            }

            TempData["Message"] = "Team registered successfully. Your registration will be complete once payment is received.";
            return View(vm);
        }

        public async Task<IActionResult> Paid()
        {
            var registrations = await _registrationGateway.GetAllPaidAsync();
            return View(registrations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamGateway.GetByIdAsync(id);
            if (team == null) return NotFound();
            return View(team);
        }
    }
}
