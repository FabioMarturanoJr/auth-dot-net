namespace AuthJwt.Domain.Auth;

public class UserToken
{
    public UserToken(bool auhenticated, DateTime expiration, string token, string message)
    {
        Auhenticated = auhenticated;
        Expiration = expiration;
        Token = token;
        Message = message;
    }

    public bool Auhenticated { get; set; }
    public DateTime Expiration { get; set; }
    public string Token { get; set; }
    public string Message { get; set; }
}
