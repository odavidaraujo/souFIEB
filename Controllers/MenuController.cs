using Microsoft.AspNetCore.Mvc;
using souFIEB.Controllers;
using Soufieb.Webapp.Models;
using System.Security.Claims;

namespace Soufieb.Webapp.Controllers
{
    public class MenuController : Controller
    {
        private readonly Contexto _contexto;
        private readonly ILogger<MenuController> _logger;

                       
        public MenuController(ILogger<MenuController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }
        public IActionResult Menu()
        {
            // Acesso ao email do usuário autenticado
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(userEmail))
            {
                ViewBag.UserEmail = userEmail;
            }

            return View("_LayoutMenu");
        }
    }
}
