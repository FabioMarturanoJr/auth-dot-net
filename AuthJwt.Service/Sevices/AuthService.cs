using AuthJwt.Domain.Auth;
using AuthJwt.Domain.Configurations;
using AuthJwt.Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenConfig _tokenConfig;

    public AuthService(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IOptions<TokenConfig> tokenConfig,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenConfig = tokenConfig.Value;
        _roleManager = roleManager;
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
        return await GerarToken(model);
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
        return await GerarToken(userInfo, true);
    }

    public async Task AddRoleUsuario(string userEmail, string role)
    {
        var usuario = await ValidaEmailERole(userEmail, role);
        await _userManager.AddToRoleAsync(usuario, role);
    }

    public async Task RemoveRoleUsuario(string userEmail, string role)
    {
        var usuario = await ValidaEmailERole(userEmail, role);
        await _userManager.RemoveFromRoleAsync(usuario, role);
    }

    public async Task<List<UserRolesDtos>> ListarUsuarios()
    {
        var usersRoles = new List<UserRolesDtos>();
        var users = await _userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            usersRoles.Add(
                new UserRolesDtos { 
                    User = user.Email, 
                    Roles = await _userManager.GetRolesAsync(user) });
        }
        return usersRoles;
    }

    public async Task<List<string>> ListarRoles()
    {
        return await _roleManager.Roles.Select(role => role.Name).ToListAsync();
    }

    private async Task<IdentityUser> ValidaEmailERole(string userEmail, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
        {
            var err = new Exception();
            err.Data["Error"] = "Role não existe";
            throw err;
        }

        var usuario = await _userManager.FindByEmailAsync(userEmail);
        if (usuario == null)
        {
            var err = new Exception();
            err.Data["Error"] = "Usuario não encontrado";
            throw err;
        }
        return usuario;
    }

    private async Task<UserToken> GerarToken(LoginUserDto userLogin, bool getRoles = false)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.UniqueName, userLogin.Email),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (getRoles)
        {
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(userLogin.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }
        }

        var expiracao = DateTime.UtcNow.AddHours(_tokenConfig.ExpireHours);
        var token = new JwtSecurityToken(
            _tokenConfig.Issuer,
            _tokenConfig.Audience,
            claims,
            null,
            expiracao,
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.JwtKey)), 
                SecurityAlgorithms.HmacSha256));

        return new UserToken(true, expiracao, new JwtSecurityTokenHandler().WriteToken(token), "Token Ok");
    }
}
