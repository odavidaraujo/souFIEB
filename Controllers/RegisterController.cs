using Microsoft.AspNetCore.Mvc;

namespace Soufieb.Webapp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;
        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }
    }
}
