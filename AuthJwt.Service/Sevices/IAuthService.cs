using AuthJwt.Domain.Auth;
using AuthJwt.Domain.Dtos;
using AuthJwt.Infrastructure.Model;

namespace AuthJwt.Service.Sevices;

public interface IAuthService
{
    Task<UserToken> RegistrarUsuario(CreateUserDto model, string baseUrl);
    Task<UserToken> Login(LoginUserDto model);
    Task AddRoleUsuario(string userEmail, string role);
    Task RemoveRoleUsuario(string userEmail, string role);
    Task<List<UserRolesDtos>> ListarUsuarios();
    Task<List<string>> ListarRoles();
    Task<CustomIdentityUser> DeleteUsuario(string id);
    Task AtualizaSenha(AtualizaSenhaDto dto);
    Task ConfirmEmail(string token, string email);
}
