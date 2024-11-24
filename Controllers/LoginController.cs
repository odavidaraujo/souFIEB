using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using souFIEB.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Soufieb.Webapp.Models;
using Microsoft.AspNetCore.Identity;

namespace souFIEB.Controllers
{
    public class LoginController : Controller
    {
        private readonly Contexto _contexto;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }


        [Route("login")]
        public IActionResult Login()
        {
            if (TempData["FailLogin"] != null && (bool)TempData["FailLogin"] == true)
            {
                ViewBag.FailLogin = true;
            }
            else
            {
                ViewBag.FailLogin = false;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ValidaLogin([FromForm] string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                TempData["FailLogin"] = true;
                return RedirectToAction("login");
            }

            var user = _contexto.User.SingleOrDefault(u => u.Email == email);
            if (user != null && user.Senha == password && user.Admin == "1" && user.Ativo == "1")
            {
                var claims = new[] {
                    new Claim(ClaimTypes.Name, email)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Efetuar login
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Redirect("/Home"); ;

            }
            else
            {
                TempData["FailLogin"] = true;
                return RedirectToAction("login");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            // Efetuar logout
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirecionar para a página de login
            return RedirectToAction("/Login");
        }
    }
}