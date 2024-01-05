using AuthJwt.Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthJwt.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("[action]")]
        public string TestApi()
        {
            return $"Api testada em {DateTime.Now.ToLongDateString()}";
            
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> RegistrarUsuario([FromBody] UserDto model)
        {

            var user = new IdentityUser 
            { 
                UserName = model.Email, 
                Email = model.Email,
                EmailConfirmed = true
            };

            if (!string.Equals(model.Password, model.ConfirmPassword, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new Exception("Confime a senha"));
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _signInManager.SignInAsync(user, false);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login([FromBody] UserDto userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, false, false);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(new Exception("Login Inválido"));
        }
    }
}
