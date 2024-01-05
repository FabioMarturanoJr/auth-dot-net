using AuthJwt.Domain.Dtos;

namespace AuthJwt.Service.Sevices;

public interface IAuthService
{
    Task RegistrarUsuario(CreateUserDto model);
    Task Login(LoginUserDto model);
}
