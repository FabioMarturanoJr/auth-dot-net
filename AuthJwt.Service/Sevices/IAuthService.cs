using AuthJwt.Domain.Auth;
using AuthJwt.Domain.Dtos;
using Microsoft.AspNetCore.Identity;

namespace AuthJwt.Service.Sevices;

public interface IAuthService
{
    Task<UserToken> RegistrarUsuario(CreateUserDto model);
    Task<UserToken> Login(LoginUserDto model);
    Task AddRoleUsuario(string userEmail, string role);
    Task RemoveRoleUsuario(string userEmail, string role);
    Task<List<UserRolesDtos>> ListarUsuarios();
    Task<List<string>> ListarRoles();
    Task<IdentityUser> DeleteUsuario(string id);
    Task AtualizaSenha(AtualizaSenhaDto dto);
}
