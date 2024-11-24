using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using souFIEB.Controllers;
using Soufieb.Webapp.Models;

namespace Soufieb.Webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Contexto _contexto;

        public HomeController(ILogger<HomeController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        [Authorize]
        public IActionResult Home()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            string userEmail = User.Identity.Name;
            var aluno = _contexto.User.FirstOrDefault(mv => mv.Email == userEmail);

            return Json(aluno);
        }
    }
}
