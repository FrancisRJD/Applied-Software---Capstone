using Microsoft.AspNetCore.Mvc;
using bowling_tournament_MVCPRoject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using bowling_tournament_MVCPRoject.ViewModels;

namespace bowling_tournament_MVCPRoject.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly BowlingDbContext _db;

        public AdminController(BowlingDbContext db)
        {
            _db = db;
        }

        private bool IsAdmin()
        {
            return User.HasClaim("IsAdmin", "true");
        }

        //I'm not 100% sure what this is meant for so I admittedly left it alone - Francis
        public IActionResult Index(int? divisionId, bool? paid)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

            List<Team> teams;

            if (divisionId.HasValue && paid.HasValue)
            {
                teams = _db.Team
                    .Include(t => t.Division)
                    .Include(t => t.Players)
                    .Where(t => t.DivisionId == divisionId.Value
                             && t.RegistrationPaid == paid.Value)
                    .OrderBy(t => t.TeamName)
                    .ToList();
            }
            else if (divisionId.HasValue)
            {
                teams = _db.Team
                    .Include(t => t.Division)
                    .Include(t => t.Players)
                    .Where(t => t.DivisionId == divisionId.Value)
                    .OrderBy(t => t.TeamName)
                    .ToList();
            }
            else if (paid.HasValue)
            {
                teams = _db.Team
                    .Include(t => t.Division)
                    .Include(t => t.Players)
                    .Where(t => t.RegistrationPaid == paid.Value)
                    .OrderBy(t => t.TeamName)
                    .ToList();
            }
            else
            {
                teams = _db.Team
                    .Include(t => t.Division)
                    .Include(t => t.Players)
                    .OrderBy(t => t.TeamName)
                    .ToList();
            }

            return View(teams);
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

        public IActionResult CreateTeam()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

            var vm = new TeamEditVm
            {
                DivisionOptions = BuildDivisionOptions()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult CreateTeam(TeamCreateVm vm)
        {
            if (!IsAdmin())
                return RedirectToAction("Denied", "Auth");

            // Ensure exactly 4 players entered
            if (vm.NewPlayers == null || vm.NewPlayers.Count != 4)
            {
                ModelState.AddModelError("", "You must enter exactly 4 players.");
                vm.DivisionOptions = BuildDivisionOptions();
                return View(vm);
            }

            if (!ModelState.IsValid)
            {
                vm.DivisionOptions = BuildDivisionOptions();
                return View(vm);
            }

            // Create new team
            var team = new Team
            {
                TeamName = vm.TeamName,
                DivisionId = vm.DivisionId,
                RegistrationPaid = false,
                PaymentDate = null
            };

            _db.Team.Add(team);
            _db.SaveChanges(); // Needed to generate TeamId

            // Save players
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

            return RedirectToAction("TeamList", "Home");

        }


        public IActionResult EditTeam(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

            var team = _db.Team
                .Include(t => t.Division)
                .Include(t => t.Players)
                .FirstOrDefault(t => t.TeamId == id);
            
            if (team == null)
            {
                TempData["Message"] = $"Team {id} not found.";
                return RedirectToAction("TeamListAdmin");
            }

            var vm = new Team
            {
                Players = team.Players,
                Division = team.Division,
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                DivisionId = team.DivisionId,
                RegistrationPaid = team.RegistrationPaid,
                PaymentDate = team.PaymentDate
            };

            return View("Edit", vm);
        }

        [HttpPost]
        public IActionResult EditTeam(TeamEditVm vm)
        {
            if (!IsAdmin())
                return RedirectToAction("Denied", "Auth");

            var team = _db.Team.Include(t => t.Players).FirstOrDefault(t => t.TeamId == vm.TeamId);

            if (team == null)
                return NotFound();

            // UPDATE TEAM INFO
            team.TeamName = vm.TeamName;
            team.DivisionId = vm.DivisionId;

            // ADD NEW PLAYERS
            if (vm.NewPlayers != null)
            {
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
            }

            _db.SaveChanges();

            return RedirectToAction("EditTeam", new { id = vm.TeamId });
        }


        public IActionResult DeleteTeam(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

            var team = _db.Team
                .Include(t => t.Division)
                .Include(t => t.Players)
                .FirstOrDefault(t => t.TeamId == id);

            if (team == null)
            {
                TempData["Message"] = $"Team {id} not found.";
                return RedirectToAction("TeamListAdmin");
            }

            return View("Delete", team);
        }


        [HttpPost]
        public IActionResult DeleteTeamConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

            var team = _db.Team
                .Include(t => t.Players)
                .FirstOrDefault(t => t.TeamId == id);

            if (team == null)
            {
                return NotFound();
            }

            _db.Team.Remove(team);
            _db.SaveChanges();

            TempData["Message"] = $"Team {id} deleted.";
            return RedirectToAction("TeamListAdmin");
        }

        [HttpPost]
        public IActionResult UpdatePlayer(PlayerEditVm vm)
        {
            if (!IsAdmin())
                return RedirectToAction("Denied", "Auth");

            var player = _db.Player.FirstOrDefault(p => p.PlayerId == vm.PlayerId);
            if (player == null)
                return NotFound();

            player.PlayerName = vm.PlayerName;
            player.City = vm.City;
            player.Province = vm.Province;
            player.Email = vm.Email;
            player.Phone = vm.Phone;

            _db.SaveChanges();

            return RedirectToAction("EditTeam", new { id = vm.TeamId });
        }


        public IActionResult AddPlayer(int teamId)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

            var team = _db.Team
                .Include(t => t.Players)
                .FirstOrDefault(t => t.TeamId == teamId);

            if (team == null)
            {
                return NotFound();
            }

            if (team.Players.Count >= 4)
            {
                TempData["Message"] = "Each team must have exactly four players.";
                return RedirectToAction("ManagePlayers", new { teamId });
            }

            var vm = new PlayerEditVm
            {
                TeamId = teamId
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult AddPlayer(PlayerEditVm vm)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

            var team = _db.Team
                .Include(t => t.Players)
                .FirstOrDefault(t => t.TeamId == vm.TeamId);

            if (team == null)
            {
                return NotFound();
            }

            if (team.Players.Count >= 4)
            {
                TempData["Message"] = "Each team must have exactly four players.";
                return RedirectToAction("ManagePlayers", new { teamId = vm.TeamId });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var player = new Player
            {
                TeamId = vm.TeamId,
                PlayerName = vm.PlayerName,
                City = vm.City,
                Province = vm.Province,
                Email = vm.Email,
                Phone = vm.Phone
            };

            _db.Player.Add(player);
            _db.SaveChanges();

            return RedirectToAction("ManagePlayers", new { teamId = vm.TeamId });
        }

        [HttpPost]
        public IActionResult RemovePlayer(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

            var player = _db.Player
                .Include(p => p.Team)
                .ThenInclude(t => t.Players)
                .FirstOrDefault(p => p.PlayerId == id);

            if (player == null)
            {
                return NotFound();
            }

            var team = player.Team;

            if (team.Players.Count == 0)
            {
                TempData["Message"] = "There are no players to remove!";
                return RedirectToAction("EditTeam", new { id = team.TeamId });
            }

            _db.Player.Remove(player);
            _db.SaveChanges();

            return RedirectToAction("EditTeam", new { id = team.TeamId });
        }

        //Swapped from POST to GET action so I can link to this via anchor. - Francis
        public IActionResult MarkPaid(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

            var team = _db.Team.FirstOrDefault(t => t.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }

            team.RegistrationPaid = true;
            team.PaymentDate = DateTime.Today;

            _db.SaveChanges();

            TempData["Message"] = $"Team '{team.TeamName}' marked as paid.";
            return RedirectToAction("TeamListAdmin");
        }


        public IActionResult Summary()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

            List<SummaryVM> summ = new List<SummaryVM>();
            const decimal RegistrationFeeAmount = 200.00m;

            foreach (Division division in _db.Division.ToList())
            {
                int teamCount = _db.Team.ToList().Where(t => t.DivisionId == division.DivisionId).Count();
                int paidTeamCount = _db.Team
                    .Where(t => t.DivisionId == division.DivisionId && t.RegistrationPaid)
                    .Count();
                decimal divisionFees = paidTeamCount * RegistrationFeeAmount;


                Console.WriteLine(division.DivisionName);
                Console.WriteLine(teamCount);
                Console.WriteLine(paidTeamCount);

                summ.Add(new SummaryVM
                {
                    DivisionName = division.DivisionName,
                    Teams = teamCount,
                    PayingTeams = paidTeamCount,
                    TotalFees = divisionFees,
                });
            }



            int totalTeams = _db.Team.Count();
            int totalPaidTeams = _db.Team.Count(t => t.RegistrationPaid);
            decimal totalFees = totalPaidTeams * RegistrationFeeAmount;
            summ.Add(new SummaryVM
            {
                DivisionName = "Overall",
                Teams = totalTeams,
                PayingTeams = totalPaidTeams,
                TotalFees = totalFees,
            });

            Console.WriteLine(summ.Count());

            return View(summ);
        }

        /*
         * Went down the route of making admin-specific variants of the details/teamslist pages since I can't
         * seem to get the elements to hide themselves when user isn't auth'ed
         */
        [HttpGet]
        public IActionResult TeamListAdmin(string? filterBy, int? division, bool? paid, string? sortBy, string? order)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }


            /*  IF NO FILTER DISPLAY THIS  */
            var teams =
                from t in _db.Team
                join d in _db.Division
                on t.DivisionId equals d.DivisionId
                orderby t.DivisionId
                select new TeamDisplayVM
                {
                    TeamId = t.TeamId,
                    TeamName = t.TeamName,
                    TeamDivision = d.DivisionName,
                    IsPaid = t.RegistrationPaid,
                    DatePaid = t.PaymentDate,
                };

            /* IF FILTER FIND OPTIONS */
            if (filterBy == "Division" && division.HasValue)
            {
                teams = teams.Where(
                    t => t.TeamDivision == _db.Division
                    .Where(d => d.DivisionId == division)
                    .Select(d => d.DivisionName)
                    .FirstOrDefault()
                    );
            }

            if (filterBy == "PaymentStatus" && paid.HasValue)
            {
                teams = teams.Where(t => t.IsPaid == paid.Value);
            }

            /* SORT THE SELECTION */
            bool descending = order == "desc";

            teams = sortBy switch
            {
                "Name" => descending ? teams.OrderByDescending(t => t.TeamName) : teams.OrderBy(t => t.TeamName),
                "PaymentStatus" => descending ? teams.OrderByDescending(t => t.IsPaid) : teams.OrderBy(t => t.IsPaid),
                "PaymentDate" => descending ? teams.OrderByDescending(t => t.DatePaid) : teams.OrderBy(t => t.DatePaid),
                _ => teams
            };

            return View(teams.ToList());
        }

        [HttpGet]
        public IActionResult DetailsAdmin(int id)
        {
            Console.WriteLine("--------------DEBUG--------------");
            Console.WriteLine(User.Identities);

            if (!IsAdmin())
            {
                return RedirectToAction("Denied", "Auth");
            }

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
                    DatePaid = teams.PaymentDate,
                };
            //Separate LINQ query so we can grab the team's players and bolt it onto the teamVM
            var playerLinq = from players in _db.Player
                             where players.TeamId == id
                             select new Player
                             {
                                 PlayerName = players.PlayerName,
                                 PlayerId = players.PlayerId,
                                 TeamId = players.TeamId,
                                 City = players.City,
                                 Province = players.Province,
                                 Email = players.Email,
                                 Phone = players.Phone,
                             };

            if (teamLinq.Count() == 0)
            //Not an issue if players can't be found, but no team found should bounce to 404
            {
                return NotFound();
            }

            TeamDisplayVM team = teamLinq.First();
            team.Players = playerLinq.ToList();

            return View(team);
        }
    }
}
