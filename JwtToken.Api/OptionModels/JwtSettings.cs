namespace JwtToken.Api.OptionModels;

public class JwtSettings
{
    public string SecretKey { get; set; }
    public int ExpireMinutes { get; set; }
}