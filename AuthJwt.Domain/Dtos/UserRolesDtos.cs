namespace AuthJwt.Domain.Dtos;

public class UserRolesDtos
{
    public string User { get; set; }
    public IList<string> Roles { get; set; }
}
