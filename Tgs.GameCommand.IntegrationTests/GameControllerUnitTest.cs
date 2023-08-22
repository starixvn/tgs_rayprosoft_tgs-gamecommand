using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
    [Fact]
    public async Task Game_Get()
    {
        var gameId = 1;
        var gameExternalId = "test";
        var gameProviderId = Guid.NewGuid();
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameRepository>();

            _ = fakeRepository.GetAsync(
                id: gameId,
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(ValueTask.FromResult<GameInfo?>(new GameInfo(
                    GameId: gameId,
                    GameCode: gameExternalId,
                    GameProviderId: gameProviderId,
                    GameProviderCode: "KY",
                    GameExternalId: gameExternalId,
                    Type: GameType.CardGame,
                    PlatformType: GamePlatformType.DesktopAndMobile,
                    Name: "Baccarat",
                    SubGame: true,
                    AvailableToBrand: false,
                    HasTurnoverOnTieBet: false,
                    Html5: true,
                    Description: "Baccarat",
                    EndpointPath: string.Empty,
                    RuleUrl: string.Empty,
                    AvailableCurrencies: null,
                    Localization: string.Empty,
                    SubGameProviderCode: "KY")));

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameGetAsync(gameId).ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(gameProviderId, actual.GameProviderId);
        Assert.Equal(gameId, actual.GameId);
        Assert.Equal(gameExternalId, actual.GameExternalId);
    }

    [Fact]
    public async Task Game_Get_GameProviderCode_ExternalGameId()
    {
        var gameId = 1;
        var gameExternalId = "test";
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "KY";
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IGetService<GameExternalQuery, GameInfo?>>();

            _ = fakeService.GetAsync(
                query: new GameExternalQuery(gameProviderCode, gameExternalId),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(ValueTask.FromResult<GameInfo?>(new GameInfo(
                    GameId: gameId,
                    GameCode: gameExternalId,
                    GameProviderId: gameProviderId,
                    GameProviderCode: gameProviderCode,
                    GameExternalId: gameExternalId,
                    Type: GameType.CardGame,
                    PlatformType: GamePlatformType.DesktopAndMobile,
                    Name: "Baccarat",
                    SubGame: true,
                    AvailableToBrand: false,
                    HasTurnoverOnTieBet: false,
                    Html5: true,
                    Description: "Baccarat",
                    EndpointPath: string.Empty,
                    RuleUrl: string.Empty,
                    AvailableCurrencies: null,
                    Localization: string.Empty,
                    SubGameProviderCode: "KY")));

            _ = services.AddSingleton(fakeService);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameGetAsync(gameProviderCode, gameExternalId).ConfigureAwait(false);

        Assert.NotNull(actual);
    }

    [Fact]
    public async Task Game_Get_GameProviderId_ExternalGameId()
    {
        var gameId = 1;
        var gameExternalId = "test";
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "KY";
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IGetService<GameGameProviderIdExternalQuery, GameInfo?>>();

            _ = fakeService.GetAsync(
                query: new GameGameProviderIdExternalQuery(gameProviderId, gameExternalId),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(ValueTask.FromResult<GameInfo?>(new GameInfo(
                    GameId: gameId,
                    GameCode: gameExternalId,
                    GameProviderId: gameProviderId,
                    GameProviderCode: gameProviderCode,
                    GameExternalId: gameExternalId,
                    Type: GameType.CardGame,
                    PlatformType: GamePlatformType.DesktopAndMobile,
                    Name: "Baccarat",
                    SubGame: true,
                    AvailableToBrand: false,
                    HasTurnoverOnTieBet: false,
                    Html5: true,
                    Description: "Baccarat",
                    EndpointPath: string.Empty,
                    RuleUrl: string.Empty,
                    AvailableCurrencies: null,
                    Localization: string.Empty,
                    SubGameProviderCode: "KY")));

            _ = services.AddSingleton(fakeService);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameGetAsync(gameProviderId, gameExternalId).ConfigureAwait(false);

        Assert.NotNull(actual);
    }

    [Fact]
    public async Task Game_Get_GameProviderId_ExternalGameId_Id()
    {
        var gameId = 1;
        var gameProviderId = Guid.NewGuid();
        var gameExternalId = "test";

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameRepository>();

            _ = fakeRepository
            .GetIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(gameExternalId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult<int?>(gameId));

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameGetIdAsync(gameProviderId, gameExternalId).ConfigureAwait(false);

        Assert.Equal(gameId, actual);
    }

    [Fact]
    public async Task Game_Get_Status()
    {
        var gameId = 1;
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IGameRepository>();

            _ = fakeRepository.GetStatusAsync(
                id: gameId,
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(GameStatus.Active);

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.GameGetStatusAsync(gameId).ConfigureAwait(false);

        Assert.Equal(GameStatus.Active, actual);
    }

    [Fact]
    public async Task Game_Query_GameProviderCode()
    {
        var gameProviderCode = "KY";
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IQueryService<GameByGameProviderCodeQuery, GameInfo>>();

            _ = fakeService.QueryAsync(
                query: new GameByGameProviderCodeQuery(gameProviderCode),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns((new GameInfo[] {
                new GameInfo(
                    GameId: 1,
                    GameCode: "baccarat",
                    GameProviderId: Guid.NewGuid(),
                    GameProviderCode: "KY",
                    GameExternalId:  "baccarat",
                    Type: GameType.CardGame,
                    PlatformType: GamePlatformType.DesktopAndMobile,
                    Name: "Baccarat",
                    SubGame: true,
                    AvailableToBrand: false,
                    HasTurnoverOnTieBet: false,
                    Html5: true,
                    Description: "Baccarat",
                    EndpointPath: string.Empty,
                    RuleUrl: string.Empty,
                    AvailableCurrencies: null,
                    Localization: string.Empty,
                    SubGameProviderCode: "KY")  }).ToAsyncEnumerable());

            _ = services.AddSingleton(fakeService);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .GameQueryAsync(gameProviderCode)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }

    [Fact]
    public async Task Game_Query_GameProviderCode_GameType()
    {
        var gameProviderCode = "KY";
        var gameType = GameType.CardGame;
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IQueryService<GameByGameProviderCodeGameTypeQuery, GameInfo>>();

            _ = fakeService.QueryAsync(
                new GameByGameProviderCodeGameTypeQuery(gameProviderCode, gameType),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns((new GameInfo[] {
                new GameInfo(
                    GameId: 1,
                    GameCode: "baccarat",
                    GameProviderId: Guid.NewGuid(),
                    GameProviderCode: "KY",
                    GameExternalId:  "baccarat",
                    Type: GameType.CardGame,
                    PlatformType: GamePlatformType.DesktopAndMobile,
                    Name: "Baccarat",
                    SubGame: true,
                    AvailableToBrand: false,
                    HasTurnoverOnTieBet: false,
                    Html5: true,
                    Description: "Baccarat",
                    EndpointPath: string.Empty,
                    RuleUrl: string.Empty,
                    AvailableCurrencies: null,
                    Localization: string.Empty,
                    SubGameProviderCode: "KY")  }).ToAsyncEnumerable());

            _ = services.AddSingleton(fakeService);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .GameQueryAsync(
                gameProviderCode,
                GameType.CardGame)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }

    [Fact]
    public async Task Game_Query_GameProviderId()
    {
        var gameProviderId = Guid.NewGuid();
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IQueryService<GameByGameProviderIdQuery, GameInfo>>();

            _ = fakeService.QueryAsync(
                query: new GameByGameProviderIdQuery(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns((new GameInfo[] {
                new GameInfo(
                    GameId: 1,
                    GameCode: "baccarat",
                    GameProviderId: gameProviderId,
                    GameProviderCode: "KY",
                    GameExternalId:  "baccarat",
                    Type: GameType.CardGame,
                    PlatformType: GamePlatformType.DesktopAndMobile,
                    Name: "Baccarat",
                    SubGame: true,
                    AvailableToBrand: false,
                    HasTurnoverOnTieBet: false,
                    Html5: true,
                    Description: "Baccarat",
                    EndpointPath: string.Empty,
                    RuleUrl: string.Empty,
                    AvailableCurrencies: null,
                    Localization: string.Empty,
                    SubGameProviderCode: "KY")  }).ToAsyncEnumerable());

            _ = services.AddSingleton(fakeService);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .GameQueryAsync(gameProviderId)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }

    [Fact]
    public async Task Game_Query_GameProviderId_Empty()
    {
        var gameProviderId = Guid.NewGuid();
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IQueryService<GameByGameProviderIdQuery, GameInfo>>();

            _ = fakeService.QueryAsync(
                query: new GameByGameProviderIdQuery(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(Enumerable.Empty<GameInfo>().ToAsyncEnumerable());

            _ = services.AddSingleton(fakeService);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .GameQueryAsync(gameProviderId)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.Any());
    }

    [Fact]
    public async Task Game_Query_GameProviderId_GameType()
    {
        var gameProviderId = Guid.NewGuid();
        var gameType = GameType.CardGame;
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IQueryService<GameByGameProviderIdGameTypeQuery, GameInfo>>();

            _ = fakeService.QueryAsync(
                new GameByGameProviderIdGameTypeQuery(gameProviderId, gameType),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns((new GameInfo[] {
                new GameInfo(
                    GameId: 1,
                    GameCode: "baccarat",
                    GameProviderId: gameProviderId,
                    GameProviderCode: "KY",
                    GameExternalId:  "baccarat",
                    Type: GameType.CardGame,
                    PlatformType: GamePlatformType.DesktopAndMobile,
                    Name: "Baccarat",
                    SubGame: true,
                    AvailableToBrand: false,
                    HasTurnoverOnTieBet: false,
                    Html5: true,
                    Description: "Baccarat",
                    EndpointPath: string.Empty,
                    RuleUrl: string.Empty,
                    AvailableCurrencies: null,
                    Localization: string.Empty,
                    SubGameProviderCode: "KY")  }).ToAsyncEnumerable());

            _ = services.AddSingleton(fakeService);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .GameQueryAsync(
                gameProviderId,
                GameType.CardGame)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
    }

    [Fact]
    public async Task Game_Query_GameProviderId_GameType_Empty()
    {
        var gameProviderId = Guid.NewGuid();
        var gameType = GameType.CardGame;
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IQueryService<GameByGameProviderIdGameTypeQuery, GameInfo>>();

            _ = fakeService.QueryAsync(
                new GameByGameProviderIdGameTypeQuery(gameProviderId, gameType),
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(Enumerable.Empty<GameInfo>().ToAsyncEnumerable());

            _ = services.AddSingleton(fakeService);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .GameQueryAsync(
                gameProviderId,
                GameType.CardGame)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.Any());
    }
}
