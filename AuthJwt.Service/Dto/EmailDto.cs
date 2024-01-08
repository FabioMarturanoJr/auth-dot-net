using MimeKit;

namespace AuthJwt.Service.Dto;

public class EmailDto
{
    public List<MailboxAddress> To { get; set; }
    public string Titulo { get; set; }
    public string Conteudo { get; set; }
    public EmailDto(List<string> to, string titulo, string conteudo)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress("email", x)));
        Titulo = titulo;
        Conteudo = conteudo;
    }
}
