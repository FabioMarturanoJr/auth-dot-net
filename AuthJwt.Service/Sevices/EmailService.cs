using AuthJwt.Domain.Configurations;
using AuthJwt.Service.Dto;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AuthJwt.Service.Sevices;

public class EmailService : IEmailService
{
    private readonly EmailConfig emailConfig;

    public EmailService(IOptions<EmailConfig> emailConfig)
    {
        this.emailConfig = emailConfig.Value;
    }

    public void EnviarEmail(EmailDto emailDto)
    {
        MimeMessage email = CriarEmail(emailDto);
        Send(email);
    }
    private MimeMessage CriarEmail(EmailDto emailDto)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Email", emailConfig.From));
        emailMessage.To.AddRange(emailDto.To);
        emailMessage.Subject = emailDto.Titulo;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = emailDto.Conteudo };

        return emailMessage;
    }

    private void Send(MimeMessage email)
    {
        using var client = new SmtpClient();

        try
        {
            client.Connect(emailConfig.SmtpServer, emailConfig.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(emailConfig.UserName, emailConfig.Password);

            client.Send(email);
        }
        catch (Exception)
        {
            throw;
        }
        finally {
            client.Disconnect(true);
            client.Dispose(); 
        }
    }

}
