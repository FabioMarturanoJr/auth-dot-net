namespace AuthJwt.Domain.Dtos;

public class UserRolesDtos
{
    public int Id { get; set; }
    public string User { get; set; }
    public IList<string> Roles { get; set; }
}
