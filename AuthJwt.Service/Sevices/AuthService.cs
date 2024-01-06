using AuthJwt.Domain.Auth;
using AuthJwt.Domain.Configurations;
using AuthJwt.Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthJwt.Service.Sevices;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly TokenConfig _tokenConfig;

    public AuthService(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IOptions<TokenConfig> tokenConfig)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenConfig = tokenConfig.Value;
    }
    public async Task<UserToken> RegistrarUsuario(CreateUserDto model)
    {
        var user = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true
        };

        if (!string.Equals(model.Password, model.ConfirmPassword, StringComparison.OrdinalIgnoreCase))
        {
            var err = new Exception();
            err.Data["Error"] = "Confime a senha";
            throw err;
        }

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var err = new Exception();
            err.Data["Error"] = result.Errors;
            throw err;
        }
        await _signInManager.SignInAsync(user, false);
        return GererToken(model);
    }

    public async Task<UserToken> Login(LoginUserDto userInfo)
    {
        var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, false, false);

        if (!result.Succeeded)
        {
            var err = new Exception();
            err.Data["Error"] = "Login Inválido";
            throw err;
        }
        return GererToken(userInfo);
    }

    private UserToken GererToken(LoginUserDto user)
    {
        var expiracao = DateTime.UtcNow.AddHours(_tokenConfig.ExpireHours);
        var token = new JwtSecurityToken(
            _tokenConfig.Issuer,
            _tokenConfig.Audience,
            new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim("Palmeras", "Não tem mundial"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            },
            null,
            expiracao,
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.JwtKey)), 
                SecurityAlgorithms.HmacSha256));

        return new UserToken(
            true, expiracao, new JwtSecurityTokenHandler().WriteToken(token), "Token Ok");
    }
}
