using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Soufieb.Webapp.Models;


namespace Soufieb.Webapp.Controllers
{
    public class FoodMenuController : Controller
    {
        private readonly ILogger<FoodMenuController> _logger;
        private readonly Contexto _contexto;

        public FoodMenuController(ILogger<InformativeController> logger, Contexto contexto)
        {
            _logger = _logger;
            _contexto = contexto;
        }

        [Authorize]
        [Route("foodmenu")]
        public IActionResult FoodMenu()
        {
            var informativos = _contexto.Cardapio
                .OrderByDescending(i => i.Data)
                .ToList();
            if (informativos == null) { return NotFound(); }
            return View(informativos);
        }

        [Authorize]
        [HttpGet("food/{id}")]
        public IActionResult UserDetails(int id)
        {
            var food = _contexto.Cardapio.FirstOrDefault(u => u.CodCardapio == id);
            if (food == null)
            {
                return NotFound(); // Retornar um erro 404 caso o usuário não seja encontrado
            }
            return Json(food);
        }

        [Authorize]
        [HttpPost]
        [Route("food-update")]
        public async Task<IActionResult> EditarCardapioPostAsync([FromForm] string cafe, string almoco, string janta, string active, int hidden_id)
        {
            var cardapio = _contexto.Cardapio.FirstOrDefault(u => u.CodCardapio == hidden_id);

            DateTime dataAtual = DateTime.Now;
            
            if (cardapio != null)
            {
                cardapio.Data = dataAtual;
                cardapio.CafeManha = cafe;
                cardapio.PratoPrincipal = almoco;
                cardapio.Janta = janta;
                cardapio.Status = active;
                cardapio.Admin = "David Araujo";

                _contexto.SaveChanges();
                return RedirectToAction("foodmenu");
            }
            else
            {
                return NotFound(); // Lidar com o usuário não encontrado
            }
        }

        [HttpPost]
        [Route("food-add")]
        public IActionResult CadastraCardapioPostAsync([FromForm] string cafe, string almoco, string janta)
        {

            string status = "1";
            DateTime dataAtual = DateTime.Now;

            var food = new Cardapio
            {
                Data = dataAtual,
                CafeManha = cafe,
                PratoPrincipal = almoco,
                Janta = janta,
                IdUnidade = 1,
                Status = status,
                Admin = "David Araujo"
            };

            // Adicione o usuário ao DbContext
            _contexto.Cardapio.Add(food);

            // Salve as alterações no banco de dados
            _contexto.SaveChanges();

            // Redirecionar para outra página ou retornar uma resposta de sucesso
            TempData["CadastroSucesso"] = true;
            Json(new { success = true });
            return Redirect("foodmenu");
        }
    }
}
