using bowling_tournament_MVCPRoject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bowling_tournament_MVCPRoject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace bowling_tournament_MVCPRoject.Controllers
{
    public class AuthController : Controller
    {
        private readonly BowlingDbContext _db;


        public AuthController(BowlingDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginVm());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            BowlingUser user = null;

            var allUsers = await _db.BowlingUser.ToListAsync();

            foreach (var u in allUsers)
            {
                if (u.UserName == vm.UserName)
                {
                    user = u;
                    break;
                }
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(vm);
            }

            var hasher = new PasswordHasher<object>();
            var result = hasher.VerifyHashedPassword(null, user.PasswordHash, vm.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(vm);
            }

            // Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            if (user.IsAdmin)
            {
                claims.Add(new Claim("IsAdmin", "true"));
            }

            var identity = new ClaimsIdentity(claims, "app-cookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("app-cookie", principal);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("app-cookie");
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Denied()
        {
            return View();
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
