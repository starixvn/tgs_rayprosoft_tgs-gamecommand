using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollecitonExtensions
{
    public static IServiceCollection AddGameCommand(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> tgsConfigure,
        Action<IServiceProvider, DbContextOptionsBuilder> betConfigure)
        => services
        .AddMemoryCache()
        .AddGameCommandCore()
        .AddTgsRepositories(tgsConfigure)
        .AddBetRepositories(betConfigure);
}
