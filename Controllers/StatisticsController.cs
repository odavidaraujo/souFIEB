using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Soufieb.Webapp.Models;

namespace Soufieb.Webapp.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly Contexto _contexto;
        public StatisticsController(ILogger<StatisticsController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        [Authorize]
        [Route("statistics")]
        public IActionResult Statistics()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetRefeicoes()
        {
            var refeicoes = _contexto.Statistics.OrderByDescending(r => r.Data).ToList(); ;
            var alunos = _contexto.User.ToList();
            var data = new
            {
                Refeicoes = refeicoes,
                Alunos = alunos
            };
            return Json(data);
        }

    }
}
