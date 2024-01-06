using AuthJwt.Domain.Auth;
using AuthJwt.Domain.Dtos;

namespace AuthJwt.Service.Sevices;

public interface IAuthService
{
    Task<UserToken> RegistrarUsuario(CreateUserDto model);
    Task<UserToken> Login(LoginUserDto model);
}
