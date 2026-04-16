using bowling_tournament_MVCPRoject.Domain.Entities;
using bowling_tournament_MVCPRoject.Domain.Services;
using bowling_tournament_MVCPRoject.UI.Queries;
using bowling_tournament_MVCPRoject.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace bowling_tournament_MVCPRoject.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITeamReadModelGateway _teamGateway;
        private readonly ITournamentReadModelGateway _tournamentGateway;
        private readonly IRegistrationReadModelGateway _registrationGateway;
        private readonly IUserAuthorizationService _authorizationService;

        public HomeController(
            ITeamReadModelGateway teamGateway,
            ITournamentReadModelGateway tournamentGateway,
            IRegistrationReadModelGateway registrationGateway,
            IUserAuthorizationService authorizationService
            )
        {
            _teamGateway = teamGateway;
            _tournamentGateway = tournamentGateway;
            _registrationGateway = registrationGateway;
            _authorizationService = authorizationService;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> TeamList()
        {
            var teams = await _teamGateway.GetAllAsync();
            return View(teams);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamGateway.GetByIdAsync(id);

            var registrations = await _registrationGateway.GetAllPaidAsync();
            var registrationsOfTeam = registrations.Where(r=> r.team.id == id).ToList();

            if (team == null)
            {
                TempData["Message"] = $"Team {id} not found.";
                return RedirectToAction("TeamList");
            }
            if (registrationsOfTeam.Count == 0 && !_authorizationService.IsAdmin(User))
                return RedirectToAction("Denied", "Auth");
            return View(team);
        }

        [HttpGet]
        public async Task<IActionResult> ViewTournaments()
        {
            var tournaments = await _tournamentGateway.GetAllWithCapacityAsync();
            return View(tournaments);
        }

        [HttpGet]
        public async Task<IActionResult> TournamentDetails(int id)
        {
            var tournament = await _tournamentGateway.GetByIdAsync(id);
            if (tournament == null) return NotFound();

            var allRegistrations = await _registrationGateway.GetAllAsync();
            var tournamentRegistrations = allRegistrations
                .Where(r => r.tournament.id == id)
                .ToList();

            var vm = new TournamentDetailsVm
            {
                Tournament = tournament,
                Registrations = tournamentRegistrations
            };

            return View(vm);
        }

        public IActionResult CannotBeFound() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}