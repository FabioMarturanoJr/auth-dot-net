using AuthJwt.Domain.Auth;
using AuthJwt.Domain.Configurations;
using AuthJwt.Domain.Dtos;
using AuthJwt.Infrastructure.Model;
using AuthJwt.Service.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace AuthJwt.Service.Sevices;

public class AuthService : IAuthService
{
    private readonly UserManager<CustomIdentityUser> _userManager;
    private readonly SignInManager<CustomIdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly TokenConfig _tokenConfig;

    private readonly IEmailService _emailService;

    public AuthService(UserManager<CustomIdentityUser> userManager,
        SignInManager<CustomIdentityUser> signInManager,
        IOptions<TokenConfig> tokenConfig,
        RoleManager<IdentityRole<int>> roleManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenConfig = tokenConfig.Value;
        _roleManager = roleManager;
        _emailService = emailService;
    }
    public async Task<UserToken> RegistrarUsuario(CreateUserDto model, string baseUrl)
    {
        var user = new CustomIdentityUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = false
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

        await EnviarEmailVeriricacao(user, baseUrl);
        return await GerarToken(model);
    }

    private async Task EnviarEmailVeriricacao(CustomIdentityUser user, string baseUrl)
    {
        var token = await  _userManager.GenerateEmailConfirmationTokenAsync(user);
        string codeEncoded = HttpUtility.UrlEncode(token);
        var link = $"{baseUrl}?token={codeEncoded}&email={user.Email}";
        var message = new EmailDto(new List<string> { user.Email }, "ConfirmationLink", link);
        _emailService.EnviarEmail(message);
    }

    public async Task ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByNameAsync(email);
        if (user != null)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var err = new Exception();
                err.Data["Error"] = result.Errors;
                throw err;
            }
        }
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
        var usuario = await ValidaEmailRole(userEmail, role);
        await _userManager.AddToRoleAsync(usuario, role);
    }

    public async Task RemoveRoleUsuario(string userEmail, string role)
    {
        var usuario = await ValidaEmailRole(userEmail, role);
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
                    Id = user.Id,
                    User = user.Email, 
                    Roles = await _userManager.GetRolesAsync(user) });
        }
        return usersRoles;
    }

    public async Task<List<string>> ListarRoles()
    {
        return await _roleManager.Roles.Select(role => role.Name).ToListAsync();
    }

    public async Task AtualizaSenha(AtualizaSenhaDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.Id);
        var result =  await _userManager.ChangePasswordAsync(user, dto.SenhaAntiga, dto.SenhaNova);
        if (!result.Succeeded)
        {
            var err = new Exception();
            err.Data["Error"] = result.Errors;
            throw err;
        }
    }

    public async Task<CustomIdentityUser> DeleteUsuario(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        await _userManager.DeleteAsync(usuario);
        return usuario;
    }

    private async Task<CustomIdentityUser> ValidaEmailRole(string userEmail, string role)
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
