using JwtToken.Api.DTOs;
using JwtToken.Api.Models;

namespace JwtToken.Api.Services;

public interface IUserService
{
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
    Task<User?> GetById(int id);
    Task CreateInitialUsers();
}