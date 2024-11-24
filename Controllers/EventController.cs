using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Soufieb.Webapp.Models;
using System.Text.RegularExpressions;

namespace Soufieb.Webapp.Controllers
{
    public class EventController : Controller
    {
        private readonly ILogger<EventController> _logger;
        private readonly Contexto _contexto;
        public EventController(ILogger<EventController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        [Authorize]
        [Route("event")]
        public IActionResult Event()
        {
            return View();
        }

        [Authorize]
        [HttpGet("event/{id}")]
        public IActionResult Event(string id)
        {
            var user = _contexto.User.FirstOrDefault(u => u.CodigoQR == id);
            if (user == null)
            {
                return NotFound(); // Retornar um erro 404 caso o usuário não seja encontrado
            }
            return Json(user);
        }

        [Authorize]
        [HttpPost]
        [Route("event/add/{id}/{rm}")]
        public IActionResult RegistrarEvent(string id, string rm)
        {
            var qrcode = _contexto.User.FirstOrDefault(u => u.CodigoQR == id);
            if (qrcode == null)
            {
                return BadRequest("Usuário não encontrado!");
            }

            DateTime dataAtual = DateTime.Now;

            string periodoDoDia;
            int horaAtual = dataAtual.Hour;
            if (horaAtual >= 5 && horaAtual < 13)
            {
                periodoDoDia = "Manhã";
            }
            else if (horaAtual >= 13 && horaAtual < 18)
            {
                periodoDoDia = "Tarde";
            }
            else
            {
                periodoDoDia = "Noite";
            }

            int limiteDiarioRefeicao = qrcode.LimiteDiarioRefeicao ?? 0;

            var registrosHoje = _contexto.Statistics
                .Where(s => s.IdAluno == rm && s.Data.Date == dataAtual.Date)
                .ToList();

            int vezesPassadasHoje = registrosHoje.Count;

            if (vezesPassadasHoje >= limiteDiarioRefeicao)
            {
                return BadRequest("Limite diário de refeições excedido");
            }

            var scan = new Statistics
            {
                Data = dataAtual,
                Periodo = periodoDoDia,
                IdAluno = rm,
                IdUnidade = 1
            };

            _contexto.Statistics.Add(scan);

            qrcode.VezesPassadas = (qrcode.VezesPassadas ?? 0) + 1;

            _contexto.SaveChanges();

            TempData["CadastroSucesso"] = true;
            return Json(new { success = true });
        }
    }
}
