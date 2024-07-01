using Microsoft.EntityFrameworkCore;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories;
using Tgs.GameCommand.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollecitonExtensions
{
	public static IServiceCollection AddBetRepositories(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> efConfigure)
		=> services
		.AddDbContext<BetContext>(efConfigure)
		.AddTransient<IGameActionRepository, GameActionRepository>()
		.Decorate<IGameActionRepository>((innerRepository, sp) => ActivatorUtilities.CreateInstance<GameActionCacheRepository>(sp, innerRepository));

	public static IServiceCollection AddTgsRepositories(
		this IServiceCollection services,
		Action<IServiceProvider, DbContextOptionsBuilder> efConfigure)
		=> services
		.AddDbContext<TgsContext>(efConfigure)
		.AddTransient<IBrandRepository, BrandRepository>()
		.Decorate<IBrandRepository>((innerRepository, sp) => ActivatorUtilities.CreateInstance<BrandCacheRepository>(sp, innerRepository))
		.AddTransient<IGameProviderConfigurationRepository, GameProviderConfigurationRepository>()
		.AddTransient<IGameProviderRepository, GameProviderRepository>()
		.Decorate<IGameProviderRepository>((innerRepository, sp) => ActivatorUtilities.CreateInstance<GameProviderCacheRepository>(sp, innerRepository))
		.AddTransient<IGameRepository, GameRepository>()
		.Decorate<IGameRepository>((innerRepository, sp) => ActivatorUtilities.CreateInstance<GameCacheRepository>(sp, innerRepository))
		.AddTransient<ILicenseeRepository, LicenseeRepository>()
		.AddTransient<IPlayerRepository, PlayerRepository>()
		.Decorate<IPlayerRepository>((innerRepository, sp) => ActivatorUtilities.CreateInstance<PlayerCacheRepository>(sp, innerRepository, TimeSpan.FromDays(1)))
		.AddTransient<IResellerRepository, ResellerRepository>()
		.AddTransient<ISettingRepository, SettingRepository>();
}
