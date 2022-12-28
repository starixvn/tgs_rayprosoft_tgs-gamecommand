using System;
using System.Linq;
using System.Threading.Tasks;
using ExpectedObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Repositories;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class BrandRepositoryUnitTest
{
    [TestMethod]
    public async Task Get_BrandCodes()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new BrandRepository(context);
        var brandCode = "GPI";
        var brand = new Brand()
        {
            Id = Guid.NewGuid(),
            Status = 0,
            Description = null,
            OperationalEmail = null,
            TimeZoneId = null,
            LicenseeId = Guid.NewGuid(),
            ClientId = Guid.NewGuid().ToString("N"),
            SecretKey = Guid.NewGuid().ToString("N"),
            ApiKey = Guid.NewGuid().ToString("N"),
            SettlementIntervalType = 1,
            PreviousSettlementFrom = DateTime.UtcNow,
            PreviousSettlementTo = DateTime.UtcNow,
            CurrentSettlementFrom = DateTime.UtcNow,
            CurrentSettlementTo = DateTime.UtcNow,
            NextSettlementFrom = DateTime.UtcNow,
            NextSettlementTo = DateTime.UtcNow,
            AutoSentPlayerBonus = false,
            AutoSentAgentBonus = false,
            Code = brandCode,
            Name = brandCode,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            SettlementDay = null,
            LogoInfo = null,
            EnableSms = false,
            SmsVendorCode = null,
            SmsVendorSettings = null,
            PlatformCharge = 0,
            CheckDividendActiveCount = false,
            EnableSmsRegisterFlow = false,
            AgentDividendType = 0,
            EnableAgentSms = false
        };

        await context.Brands.AddRangeAsync(brand).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository
            .GetBrandCodesAsync()
            .ToArrayAsync()
            .ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(brandCode, actual.First());
    }

    [TestMethod]
    public async Task Get_BrandId_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new BrandRepository(context);
        var brandId = Guid.NewGuid();
        var brandCode = "GPI";
        var brand = new Brand()
        {
            Id = brandId,
            Status = 0,
            Description = null,
            OperationalEmail = null,
            TimeZoneId = null,
            LicenseeId = Guid.NewGuid(),
            ClientId = Guid.NewGuid().ToString("N"),
            SecretKey = Guid.NewGuid().ToString("N"),
            ApiKey = Guid.NewGuid().ToString("N"),
            SettlementIntervalType = 1,
            PreviousSettlementFrom = DateTime.UtcNow,
            PreviousSettlementTo = DateTime.UtcNow,
            CurrentSettlementFrom = DateTime.UtcNow,
            CurrentSettlementTo = DateTime.UtcNow,
            NextSettlementFrom = DateTime.UtcNow,
            NextSettlementTo = DateTime.UtcNow,
            AutoSentPlayerBonus = false,
            AutoSentAgentBonus = false,
            Code = brandCode,
            Name = brandCode,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            SettlementDay = null,
            LogoInfo = null,
            EnableSms = false,
            SmsVendorCode = null,
            SmsVendorSettings = null,
            PlatformCharge = 0,
            CheckDividendActiveCount = false,
            EnableSmsRegisterFlow = false,
            AgentDividendType = 0,
            EnableAgentSms = false
        };

        await context.Brands.AddRangeAsync(brand).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.GetBrandIdAsync(brandCode).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(brandId, actual);
    }

    [TestMethod]
    public async Task Get_Currency_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
                       .UseInMemoryDatabase(databaseName: "dbo")
                       .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                       .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new BrandRepository(context);
        var brandId = Guid.NewGuid();
        string currencyCode = "CNY";
        var brandCurrencies = new BrandCurrency[]{
            new BrandCurrency()
            {
                BrandId = brandId,
                CurrencyCode = "CNY",
                BrandCurrencyCode = "CNY"
            },
            new BrandCurrency()
            {
                BrandId = brandId,
                CurrencyCode = "HKD",
                BrandCurrencyCode = "HKD"
            },
            new BrandCurrency()
            {
                BrandId = brandId,
                CurrencyCode = "USD",
                BrandCurrencyCode = "USD"
            }
        };

        await context.BrandCurrencies.AddRangeAsync(brandCurrencies).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.GetCurrencyAsync(brandId, currencyCode).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual("CNY", actual);
    }

    [TestMethod]
    public async Task Get_GameProviderConfiguration_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new BrandRepository(context);
        var brandId = Guid.NewGuid();
        var brandCode = "GPI";
        var gameProviderCode = "ky";
        var gameProviderId = Guid.NewGuid();
        var brand = new Brand()
        {
            Id = brandId,
            Status = 0,
            Description = null,
            OperationalEmail = null,
            TimeZoneId = null,
            LicenseeId = Guid.NewGuid(),
            ClientId = Guid.NewGuid().ToString("N"),
            SecretKey = Guid.NewGuid().ToString("N"),
            ApiKey = Guid.NewGuid().ToString("N"),
            SettlementIntervalType = 1,
            PreviousSettlementFrom = DateTime.UtcNow,
            PreviousSettlementTo = DateTime.UtcNow,
            CurrentSettlementFrom = DateTime.UtcNow,
            CurrentSettlementTo = DateTime.UtcNow,
            NextSettlementFrom = DateTime.UtcNow,
            NextSettlementTo = DateTime.UtcNow,
            AutoSentPlayerBonus = false,
            AutoSentAgentBonus = false,
            Code = brandCode,
            Name = brandCode,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            SettlementDay = null,
            LogoInfo = null,
            EnableSms = false,
            SmsVendorCode = null,
            SmsVendorSettings = null,
            PlatformCharge = 0,
            CheckDividendActiveCount = false,
            EnableSmsRegisterFlow = false,
            AgentDividendType = 0,
            EnableAgentSms = false
        };

        var brandGameProvider = new BrandGameProvider()
        {
            Id = Guid.NewGuid(),
            BrandId = brandId,
            GameProviderId = gameProviderId,
            Status = (short)GameProviderStatus.Active,
            GameProviderConfigurationId = Guid.NewGuid(),
            DefaultLanguageCode = "en",
            DesktopQueryString = string.Empty,
            MobileQueryString = string.Empty,
            IsManualChangeStatus = false,
        };

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

        var expected = new BrandGameProviderInfo(
            brand.Id,
            brand.Code,
            brandGameProvider.GameProviderId,
            gameProvider.Code,
            brandGameProvider.GameProviderConfigurationId,
            brandGameProvider.DefaultLanguageCode,
            brandGameProvider.DesktopQueryString,
            brandGameProvider.MobileQueryString);

        await context.Brands.AddRangeAsync(brand).ConfigureAwait(false);
        await context.BrandGameProviders.AddRangeAsync(brandGameProvider).ConfigureAwait(false);
        await context.GameProviders.AddRangeAsync(gameProvider).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.GetGameProviderConfigurationAsync(brandId, gameProviderId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        expected.ToExpectedObject().ShouldEqual(actual);
    }

    [TestMethod]
    public async Task Get_GameProviderConfigurationId_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new BrandRepository(context);
        var gameProviderId = Guid.NewGuid();
        var brandId = Guid.NewGuid();
        var gameProviderConfigurationId = Guid.NewGuid();
        var brandGameProvider = new BrandGameProvider()
        {
            Id = Guid.NewGuid(),
            GameProviderId = gameProviderId,
            BrandId = brandId,
            GameProviderConfigurationId = gameProviderConfigurationId,
            Status = 0,
            DefaultLanguageCode = string.Empty,
            DesktopQueryString = string.Empty,
            MobileQueryString = string.Empty,
            IsManualChangeStatus = false
        };

        await context.BrandGameProviders.AddRangeAsync(brandGameProvider).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.GetGameProviderConfigurationIdAsync(brandId, gameProviderId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(gameProviderConfigurationId, actual);
    }

    [TestMethod]
    public async Task Get_GameProviderStatus_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new BrandRepository(context);
        var brandId = Guid.NewGuid();
        var brandCode = "GPI";
        var gameProviderId = Guid.NewGuid();
        var brand = new Brand()
        {
            Id = brandId,
            Status = 0,
            Description = null,
            OperationalEmail = null,
            TimeZoneId = null,
            LicenseeId = Guid.NewGuid(),
            ClientId = Guid.NewGuid().ToString("N"),
            SecretKey = Guid.NewGuid().ToString("N"),
            ApiKey = Guid.NewGuid().ToString("N"),
            SettlementIntervalType = 1,
            PreviousSettlementFrom = DateTime.UtcNow,
            PreviousSettlementTo = DateTime.UtcNow,
            CurrentSettlementFrom = DateTime.UtcNow,
            CurrentSettlementTo = DateTime.UtcNow,
            NextSettlementFrom = DateTime.UtcNow,
            NextSettlementTo = DateTime.UtcNow,
            AutoSentPlayerBonus = false,
            AutoSentAgentBonus = false,
            Code = brandCode,
            Name = brandCode,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            SettlementDay = null,
            LogoInfo = null,
            EnableSms = false,
            SmsVendorCode = null,
            SmsVendorSettings = null,
            PlatformCharge = 0,
            CheckDividendActiveCount = false,
            EnableSmsRegisterFlow = false,
            AgentDividendType = 0,
            EnableAgentSms = false
        };

        var brandGameProvider = new BrandGameProvider()
        {
            Id = Guid.NewGuid(),
            BrandId = brandId,
            GameProviderId = gameProviderId,
            Status = (short)GameProviderStatus.Active,
            GameProviderConfigurationId = Guid.NewGuid(),
            DefaultLanguageCode = "en",
            DesktopQueryString = string.Empty,
            MobileQueryString = string.Empty,
            IsManualChangeStatus = false,
        };

        await context.Brands.AddRangeAsync(brand).ConfigureAwait(false);
        await context.BrandGameProviders.AddRangeAsync(brandGameProvider).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.GetGameProviderStatusAsync(brandId, gameProviderId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(GameProviderStatus.Active, actual);
    }

    [TestMethod]
    public async Task Get_GameStatus_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new BrandRepository(context);
        var brandId = Guid.NewGuid();
        var brandCode = "GPI";
        var gameProviderId = Guid.NewGuid();
        var gameId = 1000;
        var brand = new Brand()
        {
            Id = brandId,
            Status = 0,
            Description = null,
            OperationalEmail = null,
            TimeZoneId = null,
            LicenseeId = Guid.NewGuid(),
            ClientId = Guid.NewGuid().ToString("N"),
            SecretKey = Guid.NewGuid().ToString("N"),
            ApiKey = Guid.NewGuid().ToString("N"),
            SettlementIntervalType = 1,
            PreviousSettlementFrom = DateTime.UtcNow,
            PreviousSettlementTo = DateTime.UtcNow,
            CurrentSettlementFrom = DateTime.UtcNow,
            CurrentSettlementTo = DateTime.UtcNow,
            NextSettlementFrom = DateTime.UtcNow,
            NextSettlementTo = DateTime.UtcNow,
            AutoSentPlayerBonus = false,
            AutoSentAgentBonus = false,
            Code = brandCode,
            Name = brandCode,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            SettlementDay = null,
            LogoInfo = null,
            EnableSms = false,
            SmsVendorCode = null,
            SmsVendorSettings = null,
            PlatformCharge = 0,
            CheckDividendActiveCount = false,
            EnableSmsRegisterFlow = false,
            AgentDividendType = 0,
            EnableAgentSms = false
        };

        var brandGame = new BrandGame()
        {
            Id = Guid.NewGuid(),
            GameId = gameId,
            BrandId = brandId,
            Status = (short)GameStatus.Active,
            Tag = 0,
            DisplayOrder = 0,
            Rebate = false,
            EffectiveBettingPercentage = 0,
        };

        await context.Brands.AddRangeAsync(brand).ConfigureAwait(false);
        await context.BrandGames.AddRangeAsync(brandGame).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.GetGameStatusAsync(brandId, gameId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(GameStatus.Active, actual);
    }

    [TestMethod]
    public async Task Get_Status_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
             .UseInMemoryDatabase(databaseName: "dbo")
             .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
             .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new BrandRepository(context);
        var brandId = Guid.NewGuid();
        var brandCode = "GPI";
        var brand = new Brand()
        {
            Id = brandId,
            Status = (short)BrandStatus.Active,
            Description = null,
            OperationalEmail = null,
            TimeZoneId = null,
            LicenseeId = Guid.NewGuid(),
            ClientId = Guid.NewGuid().ToString("N"),
            SecretKey = Guid.NewGuid().ToString("N"),
            ApiKey = Guid.NewGuid().ToString("N"),
            SettlementIntervalType = 1,
            PreviousSettlementFrom = DateTime.UtcNow,
            PreviousSettlementTo = DateTime.UtcNow,
            CurrentSettlementFrom = DateTime.UtcNow,
            CurrentSettlementTo = DateTime.UtcNow,
            NextSettlementFrom = DateTime.UtcNow,
            NextSettlementTo = DateTime.UtcNow,
            AutoSentPlayerBonus = false,
            AutoSentAgentBonus = false,
            Code = brandCode,
            Name = brandCode,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
            SettlementDay = null,
            LogoInfo = null,
            EnableSms = false,
            SmsVendorCode = null,
            SmsVendorSettings = null,
            PlatformCharge = 0,
            CheckDividendActiveCount = false,
            EnableSmsRegisterFlow = false,
            AgentDividendType = 0,
            EnableAgentSms = false
        };

        await context.Brands.AddRangeAsync(brand).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.GetStatusAsync(brandId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(BrandStatus.Active, actual);
    }
}
