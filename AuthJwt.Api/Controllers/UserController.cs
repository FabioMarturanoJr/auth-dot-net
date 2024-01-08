using AuthJwt.Domain.Dtos;
using AuthJwt.Service.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AuthJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("[action]")]
        public string RotaValidaTokenSemRole() => $"Token validado em {DateTime.Now.ToLongDateString()}";

        [Authorize(Roles = "Admin, User")]
        [HttpGet("[action]")]
        public string RotaValidaMultiplasRoles () => $"Token validado em {DateTime.Now.ToLongDateString()}";

        [HttpPost("[action]")]
        public async Task<ActionResult> RegistrarUsuario([FromBody] CreateUserDto model)
        {
            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}/api/User/{nameof(ConfirmarEmail)}";
                return Ok(await _authService.RegistrarUsuario(model, baseUrl));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data.Count == 0 ? ex.Message : ex.Data);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> ConfirmarEmail([FromQuery] string token, [FromQuery] string email)
        {
            try
            {
                await _authService.ConfirmEmail(token, email);
                return Ok();
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
