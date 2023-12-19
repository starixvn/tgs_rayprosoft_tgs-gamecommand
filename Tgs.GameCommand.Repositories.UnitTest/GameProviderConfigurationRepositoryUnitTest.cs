using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tgs.GameCommand.Repositories;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class GameProviderConfigurationRepositoryUnitTest
{
    [TestMethod]
    public async Task Get_GameProvider_Config_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderConfigurationRepository(context);
        var gameProviderConfigId = Guid.NewGuid();
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
        var gameProviderConfig = new GameProviderConfig()
        {
            Id = gameProviderConfigId,
            Name = "Configuration KY",
            Description = "KY",
            ClientId = "KY-PG",
            SecretKey = Guid.NewGuid().ToString("N"),
            Status = 0,
            GameProviderId = gameProviderId,
            DesktopEndpoint = string.Empty,
            MobileEndpoint = string.Empty,
            ConfigurationData = string.Empty,
            IsDefault = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = "admin.enabled",
            UpdatedByUsername = "admin.enabled"
        };

        await context.GameProviders.AddRangeAsync(gameProvider).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        await context.GameProviderConfigs.AddRangeAsync(gameProviderConfig).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetAsync(gameProviderConfigId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(gameProviderConfigId, actual!.GameProviderConfigId);
        Assert.AreEqual(gameProviderId, actual!.GameProviderId);
    }

    [TestMethod]
    public async Task Get_GameProvider_Configs()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new GameProviderConfigurationRepository(context);
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
        var gameProviderConfigs = new GameProviderConfig[]{
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Configuration KY",
                Description = "KY",
                ClientId = "KY-PG",
                SecretKey = Guid.NewGuid().ToString("N"),
                Status = 0,
                GameProviderId = gameProviderId,
                DesktopEndpoint = string.Empty,
                MobileEndpoint = string.Empty,
                ConfigurationData = string.Empty,
                IsDefault = true,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                CreatedByUsername = "admin.enabled",
                UpdatedByUsername = "admin.enabled"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Configuration KY",
                Description = "KY",
                ClientId = "KY-PG",
                SecretKey = Guid.NewGuid().ToString("N"),
                Status = 0,
                GameProviderId = gameProviderId,
                DesktopEndpoint = string.Empty,
                MobileEndpoint = string.Empty,
                ConfigurationData = string.Empty,
                IsDefault = true,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                CreatedByUsername = "admin.enabled",
                UpdatedByUsername = "admin.enabled"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Configuration VR",
                Description = "VR",
                ClientId = "VR-PG",
                SecretKey = Guid.NewGuid().ToString("N"),
                Status = 0,
                GameProviderId = Guid.NewGuid(),
                DesktopEndpoint = string.Empty,
                MobileEndpoint = string.Empty,
                ConfigurationData = string.Empty,
                IsDefault = true,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                CreatedByUsername = "admin.enabled",
                UpdatedByUsername = "admin.enabled"
            },
        };
        await context.GameProviders.AddRangeAsync(gameProvider).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        await context.GameProviderConfigs.AddRangeAsync(gameProviderConfigs).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.QueryAsync("KY").ToArrayAsync().ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(2, actual.Length);
    }
}
