using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Webapp.Models;
using WebappDb;

namespace Webapp.Controllers
{
    public class AccountController : Controller
    {
        private readonly webappdbContext _context;

        public AccountController(webappdbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Проверить аргументы или открытые методы", Justification = "<Ожидание>")]
        public async Task<IActionResult> Login(LoginViewModel loginVm)
        {
            if (ModelState.IsValid)
            {
                Users user = await _context.Users.FirstOrDefaultAsync(
                    u => u.Email == loginVm.Email &&
                    u.Password == loginVm.Password).ConfigureAwait(true);

                if (user != null)
                {
                    await Authenticate(loginVm.Email).ConfigureAwait(true);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные эл. почта и/или пароль");
            }

            return View(loginVm);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Проверить аргументы или открытые методы", Justification = "<Ожидание>")]
        public async Task<IActionResult> Register(RegisterViewModel registerVm)
        {
            if (ModelState.IsValid)
            {
                Users user = await _context.Users.FirstOrDefaultAsync(
                    u => u.Email == registerVm.Email).ConfigureAwait(true);

                if (user == null)
                {
                    _context.Add(new Users { Email = registerVm.Email, Password = registerVm.Password });
                    await _context.SaveChangesAsync().ConfigureAwait(true);

                    await Authenticate(registerVm.Email).ConfigureAwait(true);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные эл. почта и/или пароль");
                }
            }

            return View(registerVm);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id)).ConfigureAwait(true);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(true);

            return RedirectToAction("Login", "Account");
        }
    }
}