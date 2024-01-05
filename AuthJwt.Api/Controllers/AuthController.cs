using AuthJwt.Domain.Dtos;
using AuthJwt.Service.Sevices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthJwt.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("[action]")]
        public string TestApi() => $"Api testada em {DateTime.Now.ToLongDateString()}";
            
        [HttpPost("[action]")]
        public async Task<ActionResult> RegistrarUsuario([FromBody] CreateUserDto model)
        {
            try
            {
                await _authService.RegistrarUsuario(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login([FromBody] LoginUserDto userInfo)
        {
            try
            {
                await _authService.Login(userInfo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data);
            }
        }
    }
}
