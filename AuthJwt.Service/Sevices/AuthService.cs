using AuthJwt.Domain.Dtos;
using Microsoft.AspNetCore.Identity;

namespace AuthJwt.Service.Sevices;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthService(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task RegistrarUsuario(CreateUserDto model)
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
    }

    public async Task Login(LoginUserDto userInfo)
    {
        var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, false, false);

        if (!result.Succeeded)
        {
            var err = new Exception();
            err.Data["Error"] = "Login Inválido";
            throw err;
        }
    }
}
