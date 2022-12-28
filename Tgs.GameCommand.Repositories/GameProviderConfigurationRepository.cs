using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class GameProviderConfigurationRepository : IGameProviderConfigurationRepository
{
    private readonly TgsContext _context;

    public GameProviderConfigurationRepository(
        TgsContext context)
        => _context = context ?? throw new ArgumentNullException(nameof(context));

    public ValueTask<GameProviderConfigInfo?> GetAsync(
        Guid gameProviderConfigId,
        CancellationToken cancellationToken = default)
        => _context.GameProviderConfigs.AsQueryable()
            .Join(_context.GameProviders.AsQueryable(),
                config => config.GameProviderId,
                gameProvider => gameProvider.Id,
                (config, gameProvider) => new
                {
                    GameProviderId = gameProvider.Id,
                    GameProviderCode = gameProvider.Code,
                    GameProviderConfigId = config.Id,
                    Status = (GameProviderConfigStatus)config.Status,
                    config.Name,
                    config.Description,
                    config.ClientId,
                    config.SecretKey,
                    config.DesktopEndpoint,
                    config.MobileEndpoint,
                    config.ConfigurationData,
                    config.IsDefault
                })
            .Where(x => x.GameProviderConfigId == gameProviderConfigId)
            .Select(data => new GameProviderConfigInfo(
                    data.GameProviderId,
                    data.GameProviderCode,
                    data.GameProviderConfigId,
                    data.Status,
                    data.Name,
                    data.Description,
                    data.ClientId,
                    data.SecretKey,
                    data.DesktopEndpoint,
                    data.MobileEndpoint,
                    data.ConfigurationData,
                    data.IsDefault))
            .ToAsyncEnumerable()
            .FirstOrDefaultAsync(
                cancellationToken: cancellationToken);

    public IAsyncEnumerable<GameProviderConfigInfo> QueryAsync(
        string gameProviderCode,
        CancellationToken cancellationToken = default)
        => _context.GameProviderConfigs.AsQueryable()
            .Join(_context.GameProviders.AsQueryable(),
                config => config.GameProviderId,
                gameProvider => gameProvider.Id,
                (config, gameProvider) => new
                {
                    GameProviderId = gameProvider.Id,
                    GameProviderCode = gameProvider.Code,
                    GameProviderConfigId = config.Id,
                    Status = (GameProviderConfigStatus)config.Status,
                    config.Name,
                    config.Description,
                    config.ClientId,
                    config.SecretKey,
                    config.DesktopEndpoint,
                    config.MobileEndpoint,
                    config.ConfigurationData,
                    config.IsDefault
                })
            .Where(x => x.GameProviderCode.ToLower() == gameProviderCode.ToLower())
            .Select(data => new GameProviderConfigInfo(
                    data.GameProviderId,
                    data.GameProviderCode,
                    data.GameProviderConfigId,
                    data.Status,
                    data.Name,
                    data.Description,
                    data.ClientId,
                    data.SecretKey,
                    data.DesktopEndpoint,
                    data.MobileEndpoint,
                    data.ConfigurationData,
                    data.IsDefault))
            .ToAsyncEnumerable();
}
