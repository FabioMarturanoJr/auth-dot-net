using AuthJwt.Domain.Dtos;
using AuthJwt.Service.Sevices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("[action]")]
        public string TestApi() => $"Api testada em {DateTime.Now.ToLongDateString()}";
            
        [HttpPost("[action]")]
        public async Task<ActionResult> RegistrarUsuario([FromBody] CreateUserDto model)
        {
            try
            {
                return Ok(await _authService.RegistrarUsuario(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data.Count == 0 ? ex.Message : ex.Data);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login([FromBody] LoginUserDto userInfo)
        {
            try
            {
                return Ok(await _authService.Login(userInfo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data.Count == 0 ? ex.Message : ex.Data);
            }
        }
    }
}
