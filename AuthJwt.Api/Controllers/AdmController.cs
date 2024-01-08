using AuthJwt.Domain.Dtos;
using AuthJwt.Service.Sevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthJwt.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdmController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AdmController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("[action]")]
        public string RotaValidaTokenComRoleAdm() => $"Token validado em {DateTime.Now.ToLongDateString()}";

        [HttpPost("[action]")]
        public async Task<ActionResult> AddRoleUsuario([FromBody] AddRoleDto addRoleDto)
        {
            try
            {
                await _authService.AddRoleUsuario(addRoleDto.Email, addRoleDto.Role);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data.Count == 0 ? ex.Message : ex.Data);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RemoveRoleUsuario([FromBody] AddRoleDto addRoleDto)
        {
            try
            {
                await _authService.RemoveRoleUsuario(addRoleDto.Email, addRoleDto.Role);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data.Count == 0 ? ex.Message : ex.Data);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<UserRolesDtos>>> ListarUsuarios() => await _authService.ListarUsuarios();

        [HttpGet("[action]")]
        public async Task<ActionResult<List<string>>> ListarRoles() => await _authService.ListarRoles();
    }
}
