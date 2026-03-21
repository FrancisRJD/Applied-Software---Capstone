using bowling_tournament_MVCPRoject.Domain.Entities;
using bowling_tournament_MVCPRoject.Persistence;
using bowling_tournament_MVCPRoject.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


// TODO
namespace bowling_tournament_MVCPRoject.UI.Controllers
{
    public class TeamController : Controller
    {
        private readonly BowlingDbContext _db;
        
        public TeamController(BowlingDbContext db)
        {
            _db = db;
        }

        private IEnumerable<SelectListItem> BuildDivisionOptions()
        {
            return _db.Division
                .OrderBy(d => d.DivisionName)
                .Select(d => new SelectListItem
                {
                    Value = d.DivisionId.ToString(),
                    Text = d.DivisionName
                })
                .ToList();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var vm = new TeamRegisterVm
            {
                DivisionOptions = BuildDivisionOptions()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Register(TeamRegisterVm vm)
        {
            vm.DivisionOptions = BuildDivisionOptions();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Biz Rules
            if (_db.Team.Any(t => t.TeamName == vm.TeamName))
            {
                ModelState.AddModelError("TeamName", "This team name is already taken.");
                return View(vm);
            }

            if (vm.Players.Count != 4)
            {
                ModelState.AddModelError("", "You need at least four players.");
                return View(vm);
            }

            var team = new Team
            {
                TeamName = vm.TeamName,
                DivisionId = vm.DivisionId,
                RegistrationPaid = false,
                PaymentDate = null
            };

            _db.Team.Add(team);
            _db.SaveChanges();

            foreach (var p in vm.Players)
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


            TempData["Message"] = "Team registered successfully. Your registration will be complete once payment is received.";
            return RedirectToAction("Thanks");
        }

        public IActionResult Paid()
        {
            var teams = _db.Team
                .Include(t => t.Division)
                .Where(t => t.RegistrationPaid)
                .OrderBy(t => t.TeamName)
                .ToList();

            return View(teams);
        }

        public IActionResult Detials(int id)
        {
            var team = _db.Team
                .Include(t => t.Division)
                .Include(t => t.Players)
                .FirstOrDefault(t => t.TeamId == id);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }
    }
}
