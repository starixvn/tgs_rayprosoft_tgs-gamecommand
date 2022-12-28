using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Repositories;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class GameRepositoryUnitTest
{
    [TestMethod]
    public async Task Get()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameRepository(context);

        var gameId = 1;
        var gameExternalId = "ky";
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "KY";
        var gameProvider = new GameProvider()
        {
            Id = gameProviderId,
            Status = 0,
            Description = string.Empty,
            AutoCloseRound = false,
            SupportTestPlayer = false,
            AllowKickPlayer = true,
            PlaceBetTimeout = 0,
            SettleBetTimeout = 0,
            UniqueId = 1,
            TransferWallet = true,
            Code = gameProviderCode,
            Name = gameProviderCode,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Localization = null,
        };
        var games = new Game[] {
                new Game(){
                    Id=gameId,
                    Status= (short)GameStatus.Active,
                    ExternalId= gameExternalId,
                    Description=string.Empty,
                    EndpointPath=string.Empty,
                    RuleUrl=string.Empty,
                    GameProviderId= gameProviderId,
                    Type=   (short)GameType.CardGame,
                    PlatformType =(short)GamePlatformType.DesktopAndMobile,
                    SubGame= true,
                    AvailableToBrand= false,
                    HasTurnoverOnTieBet=  false,
                    Html5=  false,
                    AvailableCurrencies=   string.Empty,
                    CreatedOn=DateTime.UtcNow,
                    UpdatedOn=DateTime.UtcNow,
                    CreatedByUsername=string.Empty,
                    UpdatedByUsername=string.Empty,
                    Code= "ky",
                    Name=  "ky",
                    Localization=    null,
                    SubGameProviderCode=  gameProviderCode
                },
                new Game(){
                    Id=2,
                    Status= (short)GameStatus.Active,
                    ExternalId= "VR",
                    Description=string.Empty,
                    EndpointPath=string.Empty,
                    RuleUrl=string.Empty,
                    GameProviderId= Guid.NewGuid(),
                    Type=   (short)GameType.Lottery,
                    PlatformType =(short)GamePlatformType.DesktopAndMobile,
                    SubGame= true,
                    AvailableToBrand= false,
                    HasTurnoverOnTieBet=  false,
                    Html5=  false,
                    AvailableCurrencies=   string.Empty,
                    CreatedOn=DateTime.UtcNow,
                    UpdatedOn=DateTime.UtcNow,
                    CreatedByUsername=string.Empty,
                    UpdatedByUsername=string.Empty,
                    Code= "VR",
                    Name=  "VR",
                    Localization=    null,
                    SubGameProviderCode=  "VR"
                } };

        await context.GameProviders.AddRangeAsync(gameProvider).ConfigureAwait(false);
        await context.Games.AddRangeAsync(games).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.GetAsync(gameId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(gameProviderId, actual?.GameProviderId);
        Assert.AreEqual(gameId, actual?.GameId);
        Assert.AreEqual(gameExternalId, actual?.GameExternalId);
    }

    [TestMethod]
    public async Task Query()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameRepository(context);

        var gameId = 1;
        var gameExternalId = "ky";
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "KY";
        var gameProvider = new GameProvider()
        {
            Id = gameProviderId,
            Status = 0,
            Description = string.Empty,
            AutoCloseRound = false,
            SupportTestPlayer = false,
            AllowKickPlayer = true,
            PlaceBetTimeout = 0,
            SettleBetTimeout = 0,
            UniqueId = 1,
            TransferWallet = true,
            Code = gameProviderCode,
            Name = gameProviderCode,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Localization = null,
        };
        var games = new Game[] {
                new Game(){
                    Id=gameId,
                    Status= (short)GameStatus.Active,
                    ExternalId= gameExternalId,
                    Description=string.Empty,
                    EndpointPath=string.Empty,
                    RuleUrl=string.Empty,
                    GameProviderId= gameProviderId,
                    Type=   (short)GameType.CardGame,
                    PlatformType =(short)GamePlatformType.DesktopAndMobile,
                    SubGame= true,
                    AvailableToBrand= false,
                    HasTurnoverOnTieBet=  false,
                    Html5=  false,
                    AvailableCurrencies=   string.Empty,
                    CreatedOn=DateTime.UtcNow,
                    UpdatedOn=DateTime.UtcNow,
                    CreatedByUsername=string.Empty,
                    UpdatedByUsername=string.Empty,
                    Code= "ky",
                    Name=  "ky",
                    Localization=    null,
                    SubGameProviderCode= gameProviderCode
                },
                new Game(){
                    Id=2,
                    Status= (short)GameStatus.Active,
                    ExternalId= "VR",
                    Description=string.Empty,
                    EndpointPath=string.Empty,
                    RuleUrl=string.Empty,
                    GameProviderId= Guid.NewGuid(),
                    Type=   (short)GameType.Lottery,
                    PlatformType =(short)GamePlatformType.DesktopAndMobile,
                    SubGame= true,
                    AvailableToBrand= false,
                    HasTurnoverOnTieBet=  false,
                    Html5=  false,
                    AvailableCurrencies=   string.Empty,
                    CreatedOn=DateTime.UtcNow,
                    UpdatedOn=DateTime.UtcNow,
                    CreatedByUsername=string.Empty,
                    UpdatedByUsername=string.Empty,
                    Code= "VR",
                    Name=  "VR",
                    Localization=    null,
                    SubGameProviderCode=  "VR"
                } };

        await context.GameProviders.AddRangeAsync(gameProvider).ConfigureAwait(false);
        await context.Games.AddRangeAsync(games).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.QueryAsync(gameProviderId).ToArrayAsync().ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(1, actual.Length);
        Assert.AreEqual(gameProviderId, actual.First().GameProviderId);
        Assert.AreEqual(GameType.CardGame, actual.First().Type);
    }

    [TestMethod]
    public async Task Query_GameType()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameRepository(context);

        var gameId = 1;
        var gameExternalId = "ky";
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "KY";
        var gameProvider = new GameProvider()
        {
            Id = gameProviderId,
            Status = 0,
            Description = string.Empty,
            AutoCloseRound = false,
            SupportTestPlayer = false,
            AllowKickPlayer = true,
            PlaceBetTimeout = 0,
            SettleBetTimeout = 0,
            UniqueId = 1,
            TransferWallet = true,
            Code = gameProviderCode,
            Name = gameProviderCode,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Localization = null,
        };
        var games = new Game[] {
                new Game(){
                    Id=gameId,
                    Status= (short)GameStatus.Active,
                    ExternalId= gameExternalId,
                    Description=string.Empty,
                    EndpointPath=string.Empty,
                    RuleUrl=string.Empty,
                    GameProviderId= gameProviderId,
                    Type=   (short)GameType.CardGame,
                    PlatformType =(short)GamePlatformType.DesktopAndMobile,
                    SubGame= true,
                    AvailableToBrand= false,
                    HasTurnoverOnTieBet=  false,
                    Html5=  false,
                    AvailableCurrencies=   string.Empty,
                    CreatedOn=DateTime.UtcNow,
                    UpdatedOn=DateTime.UtcNow,
                    CreatedByUsername=string.Empty,
                    UpdatedByUsername=string.Empty,
                    Code= "ky",
                    Name=  "ky",
                    Localization=    null,
                    SubGameProviderCode=  gameProviderCode
                },
                new Game(){
                    Id=2,
                    Status= (short)GameStatus.Active,
                    ExternalId= "VR",
                    Description=string.Empty,
                    EndpointPath=string.Empty,
                    RuleUrl=string.Empty,
                    GameProviderId= Guid.NewGuid(),
                    Type=   (short)GameType.Lottery,
                    PlatformType =(short)GamePlatformType.DesktopAndMobile,
                    SubGame= true,
                    AvailableToBrand= false,
                    HasTurnoverOnTieBet=  false,
                    Html5=  false,
                    AvailableCurrencies=   string.Empty,
                    CreatedOn=DateTime.UtcNow,
                    UpdatedOn=DateTime.UtcNow,
                    CreatedByUsername=string.Empty,
                    UpdatedByUsername=string.Empty,
                    Code= "VR",
                    Name=  "VR",
                    Localization=    null,
                    SubGameProviderCode=  "VR"
                } };

        await context.GameProviders.AddRangeAsync(gameProvider).ConfigureAwait(false);
        await context.Games.AddRangeAsync(games).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.QueryAsync(gameProviderId, GameType.CardGame).ToArrayAsync().ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());
    }
}
