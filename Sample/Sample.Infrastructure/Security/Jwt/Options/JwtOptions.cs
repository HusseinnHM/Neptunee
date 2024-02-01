namespace EventManagement.Infrastructure.Jwt.Options;

public class JwtOptions 
{
    public const string Jwt = "jwt";

    public string Key { get; set; }
    public TimeSpan Expire { get; set; }
}