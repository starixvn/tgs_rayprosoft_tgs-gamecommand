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
public class GameProviderRepositoryUnitTest
{
    [TestMethod]
    public async Task GameProvider_Get_GameProviderIds()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderRepository(context);
        var gameProviders = new GameProvider[]{
            new GameProvider()
            {
                Id = Guid.NewGuid(),
                Status = 0,
                Description = string.Empty,
                AutoCloseRound = false,
                SupportTestPlayer = false,
                AllowKickPlayer = true,
                PlaceBetTimeout = 0,
                SettleBetTimeout = 0,
                UniqueId = 1,
                TransferWallet = true,
                Code = "KY",
                Name = "KY",
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                CreatedByUsername = string.Empty,
                UpdatedByUsername = string.Empty,
                Localization = null
            },
            new GameProvider()
            {
                Id = Guid.NewGuid(),
                Status = 0,
                Description = string.Empty,
                AutoCloseRound = false,
                SupportTestPlayer = false,
                AllowKickPlayer = true,
                PlaceBetTimeout = 0,
                SettleBetTimeout = 0,
                UniqueId = 1,
                TransferWallet = true,
                Code = "LY",
                Name ="LY",
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                CreatedByUsername = string.Empty,
                UpdatedByUsername = string.Empty,
                Localization = null
            },
        };

        await context.GameProviders.AddRangeAsync(gameProviders).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository
                .GetGameProviderIdsAsync()
                .ToArrayAsync()
                .ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Any());
    }

    [TestMethod]
    public async Task GameProvider_Id_Get_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderRepository(context);
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "ky";
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

        await context.GameProviders.AddRangeAsync(gameProvider).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetAsync(gameProviderId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(gameProviderId, actual!.GameProviderId);
        Assert.AreEqual(gameProviderCode, actual!.GameProviderCode);
    }

    [TestMethod]
    public async Task GameProvider_Id_Get_Status_Is_Active()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderRepository(context);
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "ky";
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

        await context.GameProviders.AddRangeAsync(gameProvider).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetStatusAsync(gameProviderId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(GameProviderStatus.Active, actual);
    }

    [TestMethod]
    public async Task GameProvider_Id_Get_Status_Is_Null()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderRepository(context);
        var gameProviderCode = "ky";

        var actual = await fakeRepository.GetStatusAsync(gameProviderCode).ConfigureAwait(false);
        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task Get_GameProvider_CultureCode_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderRepository(context);
        var gameProviderId = Guid.NewGuid();
        var cultureCode = "zh-CN";
        var gameProviderLanguages = new GameProviderLanguage[] {
            new ()
            {
                GameProviderId=gameProviderId,
                CultureCode="zh-TW",
                GameProviderCultureCode="TW"
            },
            new ()
            {
                GameProviderId=gameProviderId,
                CultureCode="zh-CN",
                GameProviderCultureCode="CN"
            },
            new ()
            {
                GameProviderId=gameProviderId,
                CultureCode="en-US",
                GameProviderCultureCode="US"
            }
        };

        await context.GameProviderLanguages.AddRangeAsync(gameProviderLanguages).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetCultureCodeAsync(gameProviderId, cultureCode).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual("CN", actual);
    }

    [TestMethod]
    public async Task Get_GameProvider_Currency_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderRepository(context);
        var gameProviderId = Guid.NewGuid();
        var currencyCode = "CNY";
        var gameProviderCurrencies = new GameProviderCurrency[] {
            new ()
            {
                GameProviderId=gameProviderId,
                CurrencyCode="CNY",
                GameProviderCurrencyCode="RMB"
            },
            new ()
            {
                GameProviderId=gameProviderId,
                CurrencyCode="HKD",
                GameProviderCurrencyCode="HKD"
            },
            new ()
            {
                GameProviderId=gameProviderId,
                CurrencyCode="USD",
                GameProviderCurrencyCode="USD"
            }
        };

        await context.GameProviderCurrencies.AddRangeAsync(gameProviderCurrencies).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetCurrencyAsync(gameProviderId, currencyCode).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual("RMB", actual!.GameProviderCurrencyCode);
    }

    [TestMethod]
    public async Task Get_GameProvider_CurrencyCode_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderRepository(context);
        var gameProviderId = Guid.NewGuid();
        var currencyCode = "CNY";
        var gameProviderCurrencies = new GameProviderCurrency[] {
            new ()
            {
                GameProviderId=gameProviderId,
                CurrencyCode="CNY",
                GameProviderCurrencyCode="RMB"
            },
            new ()
            {
                GameProviderId=gameProviderId,
                CurrencyCode="HKD",
                GameProviderCurrencyCode="HKD"
            },
            new ()
            {
                GameProviderId=gameProviderId,
                CurrencyCode="USD",
                GameProviderCurrencyCode="USD"
            }
        };

        await context.GameProviderCurrencies.AddRangeAsync(gameProviderCurrencies).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetCurrencyCodeAsync(gameProviderId, currencyCode).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual("RMB", actual);
    }

    [TestMethod]
    public async Task Get_GameProviderId_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderRepository(context);
        var gameProviderId = Guid.NewGuid();
        var gameProviderCode = "ky";
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

        await context.GameProviders.AddRangeAsync(gameProvider).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetGameProviderIdAsync(gameProviderCode).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(gameProviderId, actual);
    }

    [TestMethod]
    public async Task Get_GameProviderId_Does_Not_Exist()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderRepository(context);
        var gameProviderCode = "ky";
        var gameProvider = new GameProvider()
        {
            Id = Guid.NewGuid(),
            Status = 0,
            Description = string.Empty,
            AutoCloseRound = false,
            SupportTestPlayer = false,
            AllowKickPlayer = true,
            PlaceBetTimeout = 0,
            SettleBetTimeout = 0,
            UniqueId = 1,
            TransferWallet = true,
            Code = "kk",
            Name = "kk",
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            Localization = null,
        };

        await context.GameProviders.AddRangeAsync(gameProvider).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetGameProviderIdAsync(gameProviderCode).ConfigureAwait(false);
        Assert.IsNull(actual);
    }
}
