namespace AuthJwt.Domain.Configurations;

public class TokenConfig
{
    public string JwtKey { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public int ExpireHours { get; set; }
}
