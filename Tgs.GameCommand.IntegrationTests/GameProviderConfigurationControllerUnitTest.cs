using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
    [Fact]
    public async Task GameProviderConfiguration_Get()
    {
        var gameProviderConfigId = Guid.NewGuid();
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "KY";
        var info = new GameProviderConfigInfo(
                GameProviderId: gameProviderId,
                GameProviderCode: gameProviderCode,
                GameProviderConfigId: gameProviderConfigId,
                Name: "Configuration KY",
                Description: "KY",
                ClientId: "KY-PG",
                SecretKey: Guid.NewGuid().ToString("N"),
                Status: 0,
                DesktopEndpoint: string.Empty,
                MobileEndpoint: string.Empty,
                ConfigurationData: string.Empty,
                IsDefault: true);

        await using var web = CreateTestSettingSite(services =>
            {
                var fakeRepository = Substitute.For<IGameProviderConfigurationRepository>();

                _ = fakeRepository
                .GetAsync(
                    gameProviderConfigId: Arg.Is(gameProviderConfigId),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(info);

                _ = services.AddSingleton(fakeRepository);
            });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .GameProviderConfigurationGetAsync(gameProviderConfigId)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(gameProviderId, actual.GameProviderId);
    }

    [Fact]
    public async Task GameProviderConfiguration_GetIs_Empty()
    {
        var gameProviderConfigId = Guid.NewGuid();

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameProviderConfigurationRepository>();

            _ = fakeRepository
            .GetAsync(
                gameProviderConfigId: Arg.Is(gameProviderConfigId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult<GameProviderConfigInfo?>(null));

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .GameProviderConfigurationGetAsync(gameProviderConfigId)
            .ConfigureAwait(false);

        Assert.Null(actual);
    }

    [Fact]
    public async Task GameProviderConfiguration_Query()
    {
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "KY";
        var gameProviderConfigurationInfos = new GameProviderConfigInfo[]{
            new GameProviderConfigInfo(
                GameProviderId: gameProviderId,
                GameProviderCode: gameProviderCode,
                GameProviderConfigId:  Guid.NewGuid(),
                Name: "Configuration KY",
                Description: "KY",
                ClientId: "KY-PG",
                SecretKey: Guid.NewGuid().ToString("N"),
                Status: 0,
                DesktopEndpoint: string.Empty,
                MobileEndpoint: string.Empty,
                ConfigurationData: string.Empty,
                IsDefault: true),
            new GameProviderConfigInfo(
                GameProviderId: gameProviderId,
                GameProviderCode: gameProviderCode,
                GameProviderConfigId: Guid.NewGuid(),
                Name: "Configuration KY",
                Description: "KY",
                ClientId: "KY-PG",
                SecretKey: Guid.NewGuid().ToString("N"),
                Status: 0,
                DesktopEndpoint: string.Empty,
                MobileEndpoint: string.Empty,
                ConfigurationData: string.Empty,
                IsDefault: true),
        };

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameProviderConfigurationRepository>();

            _ = fakeRepository
            .QueryAsync(
                gameProviderCode: Arg.Is(gameProviderCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderConfigurationInfos.ToAsyncEnumerable());

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .GameProviderConfigurationQueryAsync(gameProviderCode)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }

    [Fact]
    public async Task GameProviderConfiguration_Query_Is_Empty()
    {
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "test";

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameProviderConfigurationRepository>();

            _ = fakeRepository
            .QueryAsync(
                gameProviderCode: Arg.Is(gameProviderCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Enumerable.Empty<GameProviderConfigInfo>().ToAsyncEnumerable());

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .GameProviderConfigurationQueryAsync(gameProviderCode)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.Any());
    }
}
