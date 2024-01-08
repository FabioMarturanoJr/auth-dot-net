namespace AuthJwt.Domain.Dtos;

public class AtualizaSenhaDto
{
    public string Id { get; set; }
    public string SenhaAntiga {  get; set; }
    public string SenhaNova { get; set; }
}
