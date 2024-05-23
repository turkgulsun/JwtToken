using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtToken.Api.DTOs;
using JwtToken.Api.Models;
using JwtToken.Api.OptionModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace JwtToken.Api.Services;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;
    private readonly string _connectionString;
    private readonly JwtSettings _jwtSettings;

    public UserService(IConfiguration configuration, IOptions<JwtSettings> jwtSettings)
    {
        _connectionString = configuration.GetConnectionString("MongoDb")!;
        var client = new MongoClient(_connectionString);
        var database = client.GetDatabase("mongodb");
        _users = database.GetCollection<User>("Users");
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
    {
        var user = await _users.Find(u => u.Username == model.Username && u.Password == model.Password)
            .FirstOrDefaultAsync();

        if (user == null) return null;

        var token = await GenerateJwtToken(user);
        return new AuthenticateResponse(user, token);
    }

    public async Task<User?> GetById(int id)
    {
        return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    private async Task<string> GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = await Task.Run(() =>
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenHandler.CreateToken(tokenDescriptor);
        });

        return tokenHandler.WriteToken(token);
    }

    public async Task CreateInitialUsers()
    {
        var existingUsers = await _users.Find(_ => true).AnyAsync();
        if (existingUsers)
        {
            return;
        }

        // Create initial users.
        var initialUsers = new List<User>
        {
            new User { Id = 1, Username = "admin", Password = "admin" },
        };

        await _users.InsertManyAsync(initialUsers);
    }
}