using AuthJwt.Service.Dto;
using AuthJwt.Service.Sevices;
using Microsoft.AspNetCore.Mvc;

namespace AuthJwt.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;

        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        [HttpPost("[action]")]
        public ActionResult TestarEmail()
        {
            try
            {
                var email = new EmailDto(new List<string> { "" }, "teste", "teste");
                emailService.EnviarEmail(email);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data.Count == 0 ? ex.Message : ex.Data);
            }
        }
    }
}
