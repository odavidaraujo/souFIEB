using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Soufieb.Webapp.Models;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Soufieb.Webapp.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly Contexto _contexto;
        private readonly string imgurClientId = "d1012c265b4b335";
        public UsersController(ILogger<UsersController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        [Authorize]
        [Route("users")]
        public IActionResult Users()
        {
            if (TempData["CadastroSucesso"] != null)
            {
                if ((bool)TempData["CadastroSucesso"] == true)
                {
                    ViewBag.CadastroSucesso = true;
                }
                else
                {
                    ViewBag.CadastroSucesso = false;
                }
                
            }

            var users = _contexto.User.Where(u => u.RM != null)
                .OrderByDescending(i => i.DataCriacao)
                .ToList();
            
            return View(users);
        }

        [Authorize]
        [HttpGet("users/{id}")]
        public IActionResult UserDetails(string id)
        {
            var user = _contexto.User.FirstOrDefault(u => u.RM == id);
            if (user == null || user.RM == "rm")
            {
                return NotFound(); // Retornar um erro 404 caso o usuário não seja encontrado
            }
            return Json(user);
        }

        [HttpDelete("users/del/{id}")] // Define a rota para a exclusão de um usuário com um ID específico
        public IActionResult DeleteUser(string id)
        {
            try
            {
                // Aqui, você pode implementar a lógica para excluir o usuário com base no ID
                var userToDelete = _contexto.User.FirstOrDefault(u => u.RM == id);

                var firebaseCredential = GoogleCredential.FromFile("C:\\Users\\david\\OneDrive\\Área de Trabalho\\souFIE/souFIEB/Models/firebase-adminsdk.json");

                var app = FirebaseApp.DefaultInstance;

                if (app == null)
                {
                    // A instância padrão ainda não foi criada, crie-a agora
                    app = FirebaseApp.Create(new AppOptions
                    {
                        Credential = firebaseCredential
                    });
                }

                var auth = FirebaseAuth.GetAuth(app);

                try
                {
                    // Substitua 'uidDoUsuarioAExcluir' pelo UID do usuário que você deseja excluir.
                    auth.DeleteUserAsync(id).Wait();

                    // Usuário excluído com sucesso
                }
                catch (Exception ex)
                {
                    // Lide com erros aqui
                }
                TempData["CadastroSucesso"] = true;
                _contexto.User.Remove(userToDelete); // Remove o usuário do contexto
                _contexto.SaveChanges(); // Salva as alterações no banco de dados
            }
            catch (Exception ex)
            {
                TempData["CadastroSucesso"] = false;
                return BadRequest(new { message = "Erro ao excluir o usuário: " + ex.Message }); // Retorna uma resposta de erro com uma mensagem
            }
            return Ok(new { success = true, message = "Usuário excluído com sucesso." }); // Retorna uma resposta JSON de sucesso
        }

        [Authorize]
        [HttpPost]
        [Route("users-update")]
        public async Task<IActionResult> EditarUserPostAsync([FromForm] string fullname, string datebirth, string rg, string cpf, string email, string admin, string phone, string rm, string postalcode, string streetname, string streetno, string city, string state, string district, string additional, string country_residence, string active, IFormFile file)
        {
            var user = _contexto.User.FirstOrDefault(u => u.RM == rm);

            var fotoNoIMGUR = "";
            var error = "";

            if (file != null)
            {
                using (var client = new HttpClient())
                {
                    var imageBytes = new byte[file.Length];
                    using (var stream = file.OpenReadStream())
                    {
                        await stream.ReadAsync(imageBytes, 0, (int)file.Length);
                    }

                    var base64Image = Convert.ToBase64String(imageBytes);

                    var requestData = new
                    {
                        image = base64Image
                    };

                    var requestBody = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Add("Authorization", "Client-ID " + imgurClientId);

                    var response = await client.PostAsync("https://api.imgur.com/3/image", requestBody);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        dynamic imgurResponse = JsonConvert.DeserializeObject(responseContent);

                        if (imgurResponse != null && imgurResponse.data != null)
                        {
                            // A imagem foi enviada com sucesso, você pode acessar a URL da imagem em imgurResponse.data.link
                            fotoNoIMGUR = imgurResponse.data.link.ToString();
                        }
                        else
                        {
                            // Tratar erro de upload
                            error = "Erro ao fazer o upload da imagem no Imgur.";
                            ViewBag.CadastroFail = true;
                            ViewBag.ErrorMsg = error;
                            return View("CadastrarUser");
                        }
                    }
                    else
                    {
                        // Tratar erro de upload
                        error = "Erro ao fazer o upload da imagem no Imgur.";
                        ViewBag.CadastroFail = true;
                        ViewBag.ErrorMsg = error;
                        return View("CadastrarUser");
                    }
                }
            }

            if (user != null)
            {
                user.Nome = fullname;
                user.CPF = Regex.Replace(cpf, @"\D", "");
                user.RG = Regex.Replace(rg, @"\D", "");
                user.DataNascimento = DateTime.Parse(datebirth); // Certifique-se de manipular a data adequadamente
                user.Telefone = Regex.Replace(phone, @"\D", "");
                user.Email = email;
                user.Rua = streetname;
                user.Num = streetno;
                user.CEP = Regex.Replace(postalcode, @"\D", "");
                user.Bairro = district;
                user.Cidade = city;
                user.UF = state;
                user.Complemento = additional;
                if (fotoNoIMGUR != "") user.Foto = fotoNoIMGUR;
                user.Admin = admin;
                user.LimiteDiarioRefeicao = 4; // Defina o valor apropriado
                user.VezesPassadas = 0; // Defina o valor apropriado
                user.IdCurso = 1; // Defina o valor apropriado
                user.IdTurma = 1; // Defina o valor apropriado
                user.IdUnidade = 1; // Defina o valor apropriado
                user.Ativo = active;

                _contexto.SaveChanges();
                return RedirectToAction("users");
            }
            else
            {
                return NotFound(); // Lidar com o usuário não encontrado
            }
        }



        /*[HttpPost]
        [Route("atualizar-usuario")]
        public IActionResult UpdateUsuario([FromForm] UserUpdateModel updatedUser)
        {
            string action = updatedUser.Action;

            if (action == "updateUser1")
            {
                // Processar a atualização do usuário para o primeiro formulário
                // ...
            }
            else if (action == "updateUser2")
            {
                // Processar a atualização do usuário para o segundo formulário
                // ...
            }
            return action;

            // Retornar uma resposta adequada (por exemplo, um JSON indicando o sucesso)
        }


        [HttpPost]
        [Route("update-user")]
        public IActionResult AtualizarUsuarioPost([FromBody] UserModel userModel)
        {
            try
            {
                // Verificar se o usuário com o ID fornecido existe no banco de dados
                var existingUser = _contexto.User.FirstOrDefault(u => u.Id == userModel.Id);

                if (existingUser == null)
                {
                    // O usuário não foi encontrado, retorne uma resposta de erro adequada
                    return BadRequest("Usuário não encontrado.");
                }

                // Atualize as propriedades do usuário com base nos valores do modelo recebido
                existingUser.Nome = userModel.Nome;
                existingUser.CPF = userModel.CPF;
                existingUser.RG = userModel.RG;
                existingUser.DataNascimento = userModel.DataNascimento;
                existingUser.Telefone = userModel.Telefone;
                existingUser.Email = userModel.Email;
                existingUser.Rua = userModel.Rua;
                existingUser.CEP = userModel.CEP;
                existingUser.Bairro = userModel.Bairro;
                existingUser.Cidade = userModel.Cidade;
                existingUser.UF = userModel.UF;
                existingUser.Complemento = userModel.Complemento;
                existingUser.Senha = userModel.Senha;
                existingUser.Foto = userModel.Foto;
                existingUser.Admin = userModel.Admin;
                // Atualize outras propriedades conforme necessário

                // Salve as alterações no banco de dados
                _contexto.SaveChanges();

                // Retorne uma resposta de sucesso
                return Json(new { success = true, message = "Usuário atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                // Lógica para lidar com erros, como registro ou envio de detalhes do erro
                return Json(new { success = false, message = "Erro ao atualizar o usuário: " + ex.Message });
            }
        }*/
    }
}
