using Microsoft.AspNetCore.Mvc;

namespace AuthJwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("[action]")]
        public bool Login()
        {
            _logger.LogInformation("Logado com Sucesso");
            return true;
        }
    }
}
