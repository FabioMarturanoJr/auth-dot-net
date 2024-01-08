using AuthJwt.Domain.Auth;
using AuthJwt.Domain.Dtos;

namespace AuthJwt.Service.Sevices;

public interface IAuthService
{
    Task<UserToken> RegistrarUsuario(CreateUserDto model);
    Task<UserToken> Login(LoginUserDto model);
    Task AddRoleUsuario(string userEmail, string role);
    Task RemoveRoleUsuario(string userEmail, string role);
    Task<List<UserRolesDtos>> ListarUsuarios();
    Task<List<string>> ListarRoles();
}
