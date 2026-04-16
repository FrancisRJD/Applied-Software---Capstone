using bowling_tournament_MVCPRoject.Domain.Services;
using bowling_tournament_MVCPRoject.UI.Queries;
using bowling_tournament_MVCPRoject.UI.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bowling_tournament_MVCPRoject.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthReadModelGateway _authGateway;
        private readonly IAuthService _authService;

        public AuthController(IAuthReadModelGateway authGateway, IAuthService authService)
        {
            _authGateway = authGateway;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login() => View(new LoginVm());

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _authGateway.GetUserByUsernameAsync(vm.UserName);

            if (user == null || !_authService.VerifyPassword(user, vm.Password))
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            if (user.IsAdmin)
                claims.Add(new Claim(AppConstants.AuthorizationClaims.IS_ADMIN, AppConstants.AuthorizationClaims.ADMIN_VALUE));

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
        public IActionResult Denied() => View();
    }
}