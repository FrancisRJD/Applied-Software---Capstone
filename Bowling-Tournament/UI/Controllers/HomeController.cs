using bowling_tournament_MVCPRoject.UI.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITeamReadModelGateway _teamGateway;
        private readonly ITournamentReadModelGateway _tournamentGateway;

        public HomeController(
            ILogger<HomeController> logger,
            ITeamReadModelGateway teamGateway,
            ITournamentReadModelGateway tournamentGateway)
        {
            _logger = logger;
            _teamGateway = teamGateway;
            _tournamentGateway = tournamentGateway;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> TeamList()
        {
            var teams = await _teamGateway.GetAllWithStatusAsync();
            var paidTeams = teams.Where(t => t.IsPaid).ToList();
            return View(paidTeams);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamGateway.GetByIdAsync(id);
            if (team == null)
            {
                TempData["Message"] = $"Team {id} not found.";
                return RedirectToAction("TeamList");
            }
            if (!team.IsPaid && !User.HasClaim("IsAdmin", "true"))
                return RedirectToAction("Denied", "Auth");
            return View(team);
        }

        [HttpGet]
        public async Task<IActionResult> ViewTournaments()
        {
            var tournaments = await _tournamentGateway.GetAllAsync();
            return View(tournaments);
        }

        [HttpGet]
        public async Task<IActionResult> TournamentDetails(int id)
        {
            var tournament = await _tournamentGateway.GetByIdAsync(id);
            if (tournament == null) return NotFound();
            return View(tournament);
        }

        public IActionResult CannotBeFound() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}