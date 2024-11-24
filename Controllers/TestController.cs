using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Soufieb.Webapp.Models;
using System.Text.RegularExpressions;
using System.Text;

namespace Soufieb.Webapp.Controllers
{
    public class TestController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly Contexto _contexto;
        public TestController(ILogger<UsersController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }

        [Authorize]
        [Route("test")]
        public IActionResult Test()
        {
            return View();
        }
    }
}
