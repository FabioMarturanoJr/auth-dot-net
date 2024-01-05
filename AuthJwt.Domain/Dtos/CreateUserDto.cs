namespace AuthJwt.Domain.Dtos;

public class CreateUserDto : LoginUserDto
{
    public string ConfirmPassword { get; set; }
}
