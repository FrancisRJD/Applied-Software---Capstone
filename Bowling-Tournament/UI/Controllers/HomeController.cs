using bowling_tournament_MVCPRoject.Domain.Models;
using bowling_tournament_MVCPRoject.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace bowling_tournament_MVCPRoject.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BowlingDbContext _db;

        public HomeController(ILogger<HomeController> logger, BowlingDbContext bowlingDb)
        {
            _logger = logger;
            _db = bowlingDb;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TeamList()
        {
            //I've basically ripped the linq we were given in one of the Restaurant MVC EF examples for this
            //If you know SQL turns out LINQ is mostly self-explanatory. I love SQL - Francis
            var teamLinq =
                from teams in _db.Team
                join division in _db.Division
                on teams.DivisionId equals division.DivisionId
                orderby teams.DivisionId
                where teams.RegistrationPaid
                select new TeamDisplayVM
                {
                    TeamId = teams.TeamId,
                    TeamName = teams.TeamName,
                    TeamDivision = division.DivisionName,
                };
            return View(teamLinq);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {

            var teamLinq =
                from teams in _db.Team
                join division in _db.Division
                on teams.DivisionId equals division.DivisionId
                orderby teams.DivisionId
                where teams.TeamId == id
                select new TeamDisplayVM
                {
                    TeamId = teams.TeamId,
                    TeamName = teams.TeamName,
                    TeamDivision = division.DivisionName,
                    IsPaid = teams.RegistrationPaid,
                };
            //Separate LINQ query so we can grab the team's players and bolt it onto the teamVM
            var playerLinq = from players in _db.Player
                             where players.TeamId == id
                             select new Player
                             {
                                 PlayerName = players.PlayerName,
                                 City = players.City,
                                 Province = players.Province,
                                 Email = players.Email,
                                 Phone = players.Phone,
                             };

            if (teamLinq.Count() == 0)
            {
                TempData["Message"] = $"Team {id} not found.";
                return RedirectToAction("TeamList");
            }

            TeamDisplayVM team = teamLinq.First();
            team.Players = playerLinq.ToList();

            //One final guardrail to block non-authorized users from accessing teams that haven't paid - Francis
            if (!team.IsPaid && !User.HasClaim("IsAdmin", "true"))
            {
                return RedirectToAction("Denied", "Auth");
            }

            return View(team);
        }

        public IActionResult Register()
        {
            var vm = new TeamCreateVm
            {
                DivisionOptions = _db.Division
                    .OrderBy(d => d.DivisionName)
                    .Select(d => new SelectListItem
                    {
                        Value = d.DivisionId.ToString(),
                        Text = d.DivisionName
                    })
                    .ToList(),

                // REQUIRED — ensures NewPlayers exists for model-binding
                NewPlayers = new List<PlayerEditVm>()
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult Register(TeamCreateVm vm)
        {
            vm.DivisionOptions = _db.Division
                .OrderBy(d => d.DivisionName)
                .Select(d => new SelectListItem
                {
                    Value = d.DivisionId.ToString(),
                    Text = d.DivisionName
                })
                .ToList();

            // Validate 4 players
            if (vm.NewPlayers == null || vm.NewPlayers.Count != 4)
            {
                ModelState.AddModelError("", "You must enter exactly 4 players.");
                return View(vm);
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Create team
            var team = new Team
            {
                TeamName = vm.TeamName,
                DivisionId = vm.DivisionId,
                RegistrationPaid = false,
                PaymentDate = null
            };

            _db.Team.Add(team);
            _db.SaveChanges();

            // Create players
            foreach (var p in vm.NewPlayers)
            {
                var player = new Player
                {
                    TeamId = team.TeamId,
                    PlayerName = p.PlayerName,
                    City = p.City,
                    Province = p.Province,
                    Email = p.Email,
                    Phone = p.Phone
                };

                _db.Player.Add(player);
            }

            _db.SaveChanges();

            TempData["Message"] = "Team registered! Payment required to complete registration.";

            return RedirectToAction("TeamList");
        }


        //When registering a player, the teamVm is passed to the page so its TempPlayer can be used as the model
        public IActionResult RegisterPlayer(TeamCreateVm teamVm)
        {

            return View(teamVm);
        }
        public IActionResult CannotBeFound()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
