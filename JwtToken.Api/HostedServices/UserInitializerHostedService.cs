using JwtToken.Api.Services;

namespace JwtToken.Api.HostedServices;

public class UserInitializerHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public UserInitializerHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        await userService.CreateInitialUsers();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}