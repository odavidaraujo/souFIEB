using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Soufieb.Webapp.Models;
using System.Text.RegularExpressions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using Newtonsoft.Json;
using System.Text;

namespace Soufieb.Webapp.Controllers
{
    public class CadastrarUserController : Controller
    {
        private readonly ILogger<CadastrarUserController> _logger;
        private readonly Contexto _contexto;
        private readonly string imgurClientId = "d1012c265b4b335";
        public CadastrarUserController(ILogger<CadastrarUserController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        [Authorize]
        [Route("cadastrar-user")]
        public IActionResult CadastrarUser()
        {
            if (TempData["CadastroSucesso"] != null && (bool)TempData["CadastroSucesso"] == true)
            {
                ViewBag.CadastroSucesso = true;
            }
            else ViewBag.CadastroSucesso = false;

            if (TempData["CadastroFail"] != null && (bool)TempData["CadastroFail"] == true)
            {
                ViewBag.CadastroFail = true;
            }
            else ViewBag.CadastroFail = false;

            var token = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("CSRF_Token", token);

            ViewData["CSRF_Token"] = token;

            return View();
        }

        [HttpPost]
        [Route("cadastrar-user")]
        public async Task<IActionResult> CadastrarUserPostAsync([FromForm] string _token, string fullname, string datebirth, string rg, string cpf, string email, string password, string phone, string rm, string postalcode, string streetname, string streetno, string city, string state, string district, string additional, string country_residence, IFormFile file)
        {

            var sessionToken = HttpContext.Session.GetString("CSRF_Token");
            /*if (_token != sessionToken)
            {
                // Token CSRF inválido, provavelmente um ataque CSRF
                // Trate o erro ou retorne uma resposta de erro adequada
                return BadRequest("Conexão com o servidor perdida.");
            }*/

            var error = "";
            if (_contexto.User.Any(u => u.Email == email))
            {
                error = "Email já associado a outra conta!";

                ViewBag.CadastroFail = true;
                ViewBag.ErrorMsg = error;
                return View("CadastrarUser"); // Retorna a visão de cadastro
            }

            // Verificar se o CPF já está cadastrado no banco de dados
            if (_contexto.User.Any(u => u.CPF == cpf))
            {
                error = "CPF já associado a outra pessoa!";

                ViewBag.CadastroFail = true;
                ViewBag.ErrorMsg = error;

                return View("CadastrarUser"); // Retorna a visão de cadastro
            }
            if (_contexto.User.Any(u => u.RG == rg))
            {
                error = "RG já associado a outra pessoa!";

                ViewBag.CadastroFail = true;
                ViewBag.ErrorMsg = error;
                
                return View("CadastrarUser"); // Retorna a visão de cadastro
            }

            string generatedCode = GenerateUniqueQRCode();
            if(rm == null) rm = GenerateUniqueRM();
            string generatedChave = GenerateUniqueChave();

            var fotoNoIMGUR = "";

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

            string admin = "0";

            // Crie uma instância do modelo User e atribua os valores do formulário a ela
            var user = new User
            {
                Nome = fullname,
                CPF = Regex.Replace(cpf, @"\D", ""),
                RG = Regex.Replace(rg, @"\D", ""),
                DataNascimento = DateTime.Parse(datebirth), // Certifique-se de manipular a data adequadamente
                Telefone = Regex.Replace(phone, @"\D", ""),
                Email = email,
                Rua = streetname,
                Num = streetno,
                CEP = Regex.Replace(postalcode, @"\D", ""),
                Bairro = district,
                Cidade = city,
                UF = state,
                Complemento = additional,
                Senha = password,
                Foto = fotoNoIMGUR, // Substitua pelo caminho real da foto
                CodigoQR = generatedCode, // Substitua pelo valor correto
                Chave = generatedChave,
                RM = rm,
                Admin = admin,
                LimiteDiarioRefeicao = 4,
                VezesPassadas = 0,
                IdCurso = 1,
                IdTurma = 1,
                IdUnidade = 1,
                Ativo = "1",
                DataCriacao = DateTime.Now
            };

            // Adicione o usuário ao DbContext
            _contexto.User.Add(user);

            // Salve as alterações no banco de dados
            _contexto.SaveChanges();


            // Limpar o token da sessão após o processamento bem-sucedido
            HttpContext.Session.Remove("CSRF_Token");

            var firebaseCredential = GoogleCredential.FromFile("../souFIEB/Models/firebase-adminsdk.json");
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

                var newUser = auth.CreateUserAsync(new UserRecordArgs
                {
                    Email = email,
                    Password = password,
                    Uid = rm,
            }).Result;

                // Usuário criado com sucesso
            }
            catch (Exception ex)
            {
                // Lide com erros aqui
            }



            // Redirecionar para outra página ou retornar uma resposta de sucesso
            TempData["CadastroSucesso"] = true;
            Json(new { success = true });
            return Redirect("cadastrar-user");
        }

        private string GenerateUniqueQRCode()
        {
            string generatedCode;

            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            do
            {
                generatedCode = new string(Enumerable.Repeat(characters, 12)
                              .Select(s => s[new Random().Next(s.Length)]).ToArray());

            } while (_contexto.User.Any(u => u.CodigoQR == generatedCode));

            return generatedCode;
        }

        private string GenerateUniqueRM()
        {
            // Obtenha o último RM no banco de dados
            //string lastRM = _contexto.User.OrderByDescending(u => u.RM).Select(u => u.RM).FirstOrDefault();

            // Inicialize o valor mínimo para gerar RM
            int minValue = 100000;

            // Encontre o próximo número disponível como RM
            int generatedRMNumber = minValue;
            while (_contexto.User.Any(u => u.RM == generatedRMNumber.ToString()))
            {
                generatedRMNumber++;
            }

            string generatedRM = generatedRMNumber.ToString();

            return generatedRM;
        }
        private string GenerateUniqueChave()
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int chaveLength = 18;

            Random random = new Random();
            char[] chaveArray = new char[chaveLength];

            do
            {
                for (int i = 0; i < chaveLength; i++)
                {
                    chaveArray[i] = allowedChars[random.Next(0, allowedChars.Length)];
                }

                string generatedChave = new string(chaveArray);

                // Verifique se a chave gerada já existe no banco de dados
                bool chaveExists = _contexto.User.Any(u => u.Chave == generatedChave);

                if (!chaveExists)
                {
                    return generatedChave; // A chave é única, retorne-a
                }

                // Caso contrário, gere uma nova chave e verifique novamente
            } while (true);
        }

    }
}
