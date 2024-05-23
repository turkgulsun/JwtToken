using System.Text.Json.Serialization;

namespace JwtToken.Api.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    [JsonIgnore] public string Password { get; set; }
}