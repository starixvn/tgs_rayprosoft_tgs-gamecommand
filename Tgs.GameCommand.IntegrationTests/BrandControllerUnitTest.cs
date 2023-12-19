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
    public async Task Brand_Get_BrandId()
    {
        var brandCode = "GPI";
        var brandId = Guid.NewGuid();
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IBrandRepository>();

            _ = fakeRepository.GetBrandIdAsync(
                brandCode: Arg.Is(brandCode),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(brandId);
            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.BrandGetBrandIdAsync(brandCode).ConfigureAwait(false);

        Assert.Equal(brandId, actual);
    }

    [Fact]
    public async Task Brand_Get_Currency()
    {
        var currencyCode = "CNY";
        var brandId = Guid.NewGuid();
        var brandCurrencyCode = "CNY";
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IBrandRepository>();

            _ = fakeRepository
            .GetCurrencyAsync(
                brandId: Arg.Is(brandId),
                currencyCode: Arg.Is(currencyCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(brandCurrencyCode);

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.BrandGetCurrencyAsync(brandId, currencyCode).ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(brandCurrencyCode, actual);
    }

    [Fact]
    public async Task Brand_Get_GameProviderConfiguration()
    {
        var brandId = Guid.NewGuid();
        var gameProviderId = Guid.NewGuid();
        var gameProviderConfigurationId = Guid.NewGuid();
        var info = new BrandGameProviderInfo(
            BrandId: brandId,
            BrandCode: "GPI",
            GameProviderId: gameProviderId,
            GameProviderCode: "KY",
            GameProviderConfigurationId: gameProviderConfigurationId,
            DefaultLanguageCode: "cn",
            DesktopQueryString: string.Empty,
            MobileQueryString: string.Empty);
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IBrandRepository>();

            _ = fakeRepository.GetGameProviderConfigurationAsync(
                brandId: Arg.Is(brandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(info);

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.BrandGetGameProviderConfigurationAsync(brandId, gameProviderId).ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(brandId, actual.BrandId);
        Assert.Equal(gameProviderId, actual.GameProviderId);
        Assert.Equal(gameProviderConfigurationId, actual.GameProviderConfigurationId);
    }

    [Fact]
    public async Task Brand_Get_GameProviderConfigurationId()
    {
        var brandId = Guid.NewGuid();
        var gameProviderId = Guid.NewGuid();
        var gameProviderConfigurationId = Guid.NewGuid();
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IBrandRepository>();

            _ = fakeRepository.GetGameProviderConfigurationIdAsync(
                brandId: Arg.Is(brandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(gameProviderConfigurationId);

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.BrandGetGameProviderConfigurationIdAsync(brandId, gameProviderId).ConfigureAwait(false);

        Assert.Equal(gameProviderConfigurationId, actual);
    }

    [Fact]
    public async Task Brand_Get_GameProviderStatus()
    {
        var expected = GameProviderStatus.Active;
        var brandId = Guid.NewGuid();
        var gameProviderId = Guid.NewGuid();
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IBrandRepository>();

            _ = fakeRepository
            .GetGameProviderStatusAsync(
                brandId: Arg.Is(brandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(GameProviderStatus.Active);

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.BrandGetGameProviderStatusAsync(brandId, gameProviderId).ConfigureAwait(false);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Brand_Get_GameStatus()
    {
        var expected = GameStatus.Active;
        var brandId = Guid.NewGuid();
        var gameId = 1000;
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IBrandRepository>();

            _ = fakeRepository
            .GetGameStatusAsync(
                brandId: Arg.Is(brandId),
                gameId: Arg.Is(gameId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(GameStatus.Active);

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.BrandGetGameStatusAsync(brandId, gameId).ConfigureAwait(false);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Brand_Get_Status()
    {
        var expected = BrandStatus.Active;
        var brandId = Guid.NewGuid();
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IBrandRepository>();

            _ = fakeRepository
            .GetStatusAsync(
                brandId: Arg.Is(brandId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(BrandStatus.Active);

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.BrandGetStatusAsync(brandId).ConfigureAwait(false);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Brand_Query()
    {
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IBrandRepository>();

            _ = fakeRepository
            .QueryAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(new BrandInfo[]
            {
                new(
                    Guid.NewGuid(),
                    "TGS",
                    Guid.NewGuid(),
                    "TGS",
                    Guid.NewGuid(),
                    "GPI"
                    )
            }.ToAsyncEnumerable());

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .BrandQueryAsync()
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<BrandInfo>();

        Assert.True(actual.Any());
    }

    [Fact]
    public async Task Brand_Query_GameProvider()
    {
        var brandId = Guid.NewGuid();
        var gameProviderId = Guid.NewGuid();
        var gameProviderConfigurationId = Guid.NewGuid();

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IBrandRepository>();

            _ = fakeRepository.QueryGameProviderAsync(
                brandId: Arg.Is(brandId),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(new BrandGameProviderInfo[] {
                    new(
                        BrandId: brandId,
                        BrandCode: "GPI",
                        GameProviderId: gameProviderId,
                        GameProviderCode: "KY",
                        GameProviderConfigurationId: gameProviderConfigurationId,
                        DefaultLanguageCode: "cn",
                        DesktopQueryString: string.Empty,
                        MobileQueryString: string.Empty)
                }.ToAsyncEnumerable());

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .BrandQueryGameProviderAsync(brandId)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<BrandGameProviderInfo>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }
}
