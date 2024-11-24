using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soufieb.Webapp.Models;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace Soufieb.Webapp.Controllers
{
    public class InformativeController : Controller
    {
        private readonly ILogger<InformativeController> _logger;
        private readonly Contexto _contexto;

        public InformativeController(ILogger<InformativeController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        [Authorize]
        [Route("informative")]
        public IActionResult Informative()
        {
            var informativos = _contexto.Informatives
                .OrderByDescending(i => i.DataCriacao)
                .ToList();

            foreach (var informativo in informativos)
            {
                int numeroLeituras = _contexto.Leitura.Count(l => l.IdInformativo == informativo.CodigoInformativo);
                informativo.NumeroLeituras = numeroLeituras;

                var totalLeituras = informativos.Sum(i => i.NumeroLeituras);
                ViewBag.TotalLeituras = totalLeituras;
            }
            

            return View(informativos);
        }

        [Authorize]
        [HttpGet]
        [Route("informative/getinfo")]
        public IActionResult GetInfo()
        {
            int infoCount = _contexto.Informatives.Count();
            var lastDate = _contexto.Informatives
                .OrderByDescending(i => i.DataCriacao)
                .Select(i => i.DataCriacao)
                .FirstOrDefault();


            return Json(new { TotalCount = infoCount, lastData = lastDate });
        }

        [Authorize]
        [HttpGet("informative/{id}")]
        public IActionResult UserDetails(int id)
        {
            var user = _contexto.Informatives.FirstOrDefault(u => u.CodigoInformativo == id);
            if (user == null)
            {
                return NotFound(); // Retornar um erro 404 caso o usuário não seja encontrado
            }
            return Json(user);
        }

        [Authorize]
        [HttpPost]
        [Route("informative-update")]
        public async Task<IActionResult> EditarInformativePostAsync([FromForm] int hidden_id, string adminEdit, string tituloEdit, string dataEdit, string mensagemEdit, int active)
        {
            var informative = _contexto.Informatives.FirstOrDefault(u => u.CodigoInformativo == hidden_id);


            if (informative != null)
            {
                informative.Admin = adminEdit;
                informative.Titulo = tituloEdit;
                informative.Mensagem = mensagemEdit;
                informative.Status = active;

                _contexto.SaveChanges();
                return RedirectToAction("informative");
            }
            else
            {
                return NotFound(); // Lidar com o usuário não encontrado
            }
        }

        [HttpPost]
        [Route("informative-add")]
        public async Task<IActionResult> CadastrarInformativePostAsync([FromForm] string name, string titulo, string mensagem)
        {

            string admin = "0";
            DateTime dataAtual = DateTime.Now;

            string dataFormatada = dataAtual.ToString("ddd, dd 'de' MMMM 'de' yyyy HH:mm");
            
            name = "enviado por " + name;


            // Crie uma instância do modelo User e atribua os valores do formulário a ela
            var informatives = new Informatives
            {
                Admin = name,
                Titulo = titulo,
                Mensagem = mensagem,
                Data = dataFormatada,
                Status = 1,
                Checked = 0,
                DataCriacao = DateTime.Now
            };

            // Adicione o usuário ao DbContext
            _contexto.Informatives.Add(informatives);

            // Salve as alterações no banco de dados
            _contexto.SaveChanges();

            // Redirecionar para outra página ou retornar uma resposta de sucesso
            TempData["CadastroSucesso"] = true;
            Json(new { success = true });
            return Redirect("informative");
        }


        /*[HttpDelete("informative/del/{id}")] // Define a rota para a exclusão de um usuário com um ID específico
        public IActionResult DeleteInfo(int id)
        {
            try
            {
                // Aqui, você pode implementar a lógica para excluir o usuário com base no ID
                var infoToDelete = _contexto.Informatives.FirstOrDefault(u => u.CodigoInformativo == id);
                TempData["CadastroSucesso"] = true;

                _contexto.Informatives.Remove(infoToDelete); // Remove o usuário do contexto
                _contexto.SaveChanges(); // Salva as alterações no banco de dados
            }
            catch (Exception ex)
            {
                TempData["CadastroSucesso"] = false;
                return BadRequest(new { message = "Erro ao excluir o usuário: " + ex.Message }); // Retorna uma resposta de erro com uma mensagem
            }
            return Ok(new { success = true, message = "Usuário excluído com sucesso." }); // Retorna uma resposta JSON de sucesso
        }*/

    }
}
