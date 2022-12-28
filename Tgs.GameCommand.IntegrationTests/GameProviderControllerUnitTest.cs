using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpectedObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;
using Tgs.GameCommand.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
    [TestMethod]
    public async Task GameProvider_Get()
    {
        var gameProviderCode = "ky";

        var gameProviderId = Guid.NewGuid();

        var expected = new GameProviderInfo(
            GameProviderId: gameProviderId,
            Description: string.Empty,
            AutoCloseRound: false,
            SupportTestPlayer: false,
            AllowKickPlayer: true,
            PlaceBetTimeout: 0,
            SettleBetTimeout: 0,
            UniqueId: 1,
            TransferWallet: true,
            GameProviderCode: gameProviderCode,
            Name: gameProviderCode,
            Localization: null);

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IGetService<GameProviderQuery, GameProviderInfo?>>();

            var query = new GameProviderQuery(
                Id: gameProviderId);

            _ = fakeService
                .GetAsync(
                    query,
                    Arg.Any<CancellationToken>())
                .Returns(ValueTask.FromResult<GameProviderInfo?>(expected));

            _ = services.AddSingleton(fakeService);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameProviderGetAsync(gameProviderId).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        expected.ToExpectedObject().ShouldEqual(actual);
    }

    [TestMethod]
    public async Task GameProvider_Get_CultureCode()
    {
        var gameProviderId = Guid.NewGuid();
        var cultureCode = "zh-CN";
        var gameProviderCultureCode = "CN";

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameProviderRepository>();

            _ = fakeRepository
                .GetCultureCodeAsync(
                    gameProviderId,
                    cultureCode,
                    Arg.Any<CancellationToken>())
                .Returns(ValueTask.FromResult<string?>(gameProviderCultureCode));

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameProviderGetCultureCodeAsync(gameProviderId, cultureCode).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(gameProviderCultureCode, actual);
    }

    [TestMethod]
    public async Task GameProvider_Get_Currency()
    {
        var gameProviderId = Guid.NewGuid();
        var currencyCode = "CNY";
        var gameProviderCurrencyCode = "RMB";
        var gameProviderCurrencyInfo = new Core.Models.GameProviderCurrencyModel(
            gameProviderId,
            currencyCode,
            gameProviderCurrencyCode
            );
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameProviderRepository>();

            _ = fakeRepository
                .GetCurrencyAsync(
                    gameProviderId,
                    currencyCode,
                    Arg.Any<CancellationToken>())
                .Returns(ValueTask.FromResult<GameProviderCurrencyModel?>(gameProviderCurrencyInfo));
            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameProviderGetCurrencyAsync(gameProviderId, currencyCode).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(currencyCode, actual.CurrencyCode);
        Assert.AreEqual(gameProviderCurrencyCode, actual.GameProviderCurrencyCode);
    }

    [TestMethod]
    public async Task GameProvider_Get_CurrencyCode()
    {
        var gameProviderId = Guid.NewGuid();
        var currencyCode = "CNY";
        var gameProviderCurrencyCode = "RMB";

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameProviderRepository>();

            _ = fakeRepository
                .GetCurrencyCodeAsync(
                    gameProviderId,
                    currencyCode,
                    Arg.Any<CancellationToken>())
                .Returns(ValueTask.FromResult<string?>(gameProviderCurrencyCode));
            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameProviderGetCurrencyCodeAsync(gameProviderId, currencyCode).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(gameProviderCurrencyCode, actual);
    }

    [TestMethod]
    public async Task GameProvider_Get_GameProviderId()
    {
        var gameProviderCode = "ky";

        var gameProviderId = Guid.NewGuid();

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameProviderRepository>();

            _ = fakeRepository
                .GetGameProviderIdAsync(
                    gameProviderCode,
                    Arg.Any<CancellationToken>())
                .Returns(ValueTask.FromResult<Guid?>(gameProviderId));

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameProviderGetGameProviderIdAsync(gameProviderCode).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(gameProviderId, actual);
    }

    [TestMethod]
    public async Task GameProvider_Get_GameProviderIds()
    {
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameProviderRepository>();

            var gameProviderIds = new[]
            {
                new GameProviderIdInfo(
                    GameProviderCode: "KY",
                    GameProviderId: Guid.NewGuid()
                ),
                new GameProviderIdInfo(
                    GameProviderCode: "LY",
                    GameProviderId: Guid.NewGuid()
                ),
            };

            _ = fakeRepository
                .GetGameProviderIdsAsync(
                    Arg.Any<CancellationToken>())
                .Returns(gameProviderIds.ToAsyncEnumerable());

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
                .GameProviderGetGameProviderIdsAsync()
                .ToArrayAsync()
                .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());
    }

    [TestMethod]
    public async Task GameProvider_Id_GetStatus()
    {
        var gameProviderId = Guid.NewGuid();

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameProviderRepository>();

            _ = fakeRepository
                .GetStatusAsync(
                    gameProviderId,
                    Arg.Any<CancellationToken>())
                .Returns(GameProviderStatus.Active);

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameProviderGetStatusAsync(gameProviderId).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(GameProviderStatus.Active, actual);
    }
}
