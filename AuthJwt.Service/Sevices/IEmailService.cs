using AuthJwt.Service.Dto;

namespace AuthJwt.Service.Sevices;

public interface IEmailService
{
    void EnviarEmail(EmailDto emailDto);
}
