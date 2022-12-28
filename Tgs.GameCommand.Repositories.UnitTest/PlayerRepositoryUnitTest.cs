using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Repositories;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class PlayerRepositoryUnitTest
{
    [TestMethod]
    public async Task Get_Already_Does_Not_Exist()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new PlayerRepository(context);

        var resellerId = Guid.NewGuid();
        var resellerCode = "TGS";
        var reseller = new Reseller()
        {
            Id = resellerId,
            Code = resellerCode,
            Name = resellerCode,
            CreatedOn = DateTime.Now,
            CreatedByUsername = string.Empty,
            UpdatedOn = DateTime.Now,
            UpdatedByUsername = string.Empty,
        };
        var licenseeId = Guid.NewGuid();
        var licenseeCode = "TGS";
        var licensee = new Licensee()
        {
            Id = licenseeId,
            Code = licenseeCode,
            Name = licenseeCode,
            ResellerId = resellerId,
            CreatedOn = DateTime.Now,
            CreatedByUsername = string.Empty,
            UpdatedOn = DateTime.Now,
            UpdatedByUsername = string.Empty,
        };
        var brandId = Guid.NewGuid();
        var brandCode = "GPI";
        var brand = new Brand()
        {
            Id = brandId,
            Code = brandCode,
            Name = brandCode,
            LicenseeId = licenseeId,
            CreatedOn = DateTime.Now,
            CreatedByUsername = string.Empty,
            UpdatedOn = DateTime.Now,
            UpdatedByUsername = string.Empty,
            ApiKey = Guid.NewGuid().ToString("N"),
            SecretKey = Guid.NewGuid().ToString("N"),
            ClientId = Guid.NewGuid().ToString("N"),
        };
        var playerCode = "4ucmqplxb029";
        var player = new Player()
        {
            PlayerCode = playerCode,
            Username = playerCode,
            BrandId = brandId,
            Status = 0,
            RealName = playerCode,
            Password = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            WithdrawPassword = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            WithdrawSalt = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            Salt = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            DefaultWalletCurrencyCode = "CNY",
            DefaultWalletId = Guid.NewGuid(),
            UpdatedOn = DateTime.Now,
            UpdatedByUsername = string.Empty,
        };
        await context.Resellers.AddRangeAsync(reseller).ConfigureAwait(false);
        await context.Licensees.AddRangeAsync(licensee).ConfigureAwait(false);
        await context.Brands.AddRangeAsync(brand).ConfigureAwait(false);
        await context.Players.AddRangeAsync(player).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetAsync("4ucmqplxb030").ConfigureAwait(false);
        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task Get_Already_Exists()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new PlayerRepository(context);

        var resellerId = Guid.NewGuid();
        var resellerCode = "TGS";
        var reseller = new Reseller()
        {
            Id = resellerId,
            Code = resellerCode,
            Name = resellerCode,
            CreatedOn = DateTime.Now,
            CreatedByUsername = string.Empty,
            UpdatedOn = DateTime.Now,
            UpdatedByUsername = string.Empty,
        };
        var licenseeId = Guid.NewGuid();
        var licenseeCode = "TGS";
        var licensee = new Licensee()
        {
            Id = licenseeId,
            Code = licenseeCode,
            Name = licenseeCode,
            ResellerId = resellerId,
            CreatedOn = DateTime.Now,
            CreatedByUsername = string.Empty,
            UpdatedOn = DateTime.Now,
            UpdatedByUsername = string.Empty,
        };
        var brandId = Guid.NewGuid();
        var brandCode = "GPI";
        var brand = new Brand()
        {
            Id = brandId,
            Code = brandCode,
            Name = brandCode,
            LicenseeId = licenseeId,
            CreatedOn = DateTime.Now,
            CreatedByUsername = string.Empty,
            UpdatedOn = DateTime.Now,
            UpdatedByUsername = string.Empty,
            ApiKey = Guid.NewGuid().ToString("N"),
            SecretKey = Guid.NewGuid().ToString("N"),
            ClientId = Guid.NewGuid().ToString("N"),
        };
        var playerCode = "4ucmqplxb029";
        var player = new Player()
        {
            PlayerCode = playerCode,
            Username = playerCode,
            Status = 0,
            BrandId = brandId,
            RealName = playerCode,
            Password = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            WithdrawPassword = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            WithdrawSalt = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            Salt = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            DefaultWalletCurrencyCode = "CNY",
            DefaultWalletId = Guid.NewGuid(),
            UpdatedOn = DateTime.Now,
            UpdatedByUsername = string.Empty,
        };
        await context.Resellers.AddRangeAsync(reseller).ConfigureAwait(false);
        await context.Licensees.AddRangeAsync(licensee).ConfigureAwait(false);
        await context.Brands.AddRangeAsync(brand).ConfigureAwait(false);
        await context.Players.AddRangeAsync(player).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetAsync(playerCode).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(resellerId, actual!.ResellerId);
        Assert.AreEqual(licenseeId, actual!.LicenseeId);
        Assert.AreEqual(brandId, actual!.BrandId);
        Assert.AreEqual(playerCode, actual!.PlayerCode);
    }

    [TestMethod]
    public async Task Get_Status_Is_Enabled()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new PlayerRepository(context);

        var brandId = Guid.NewGuid();
        var playerCode = "4ucmqplxb029";
        var player = new Player()
        {
            PlayerCode = playerCode,
            Status = 0,
            Username = playerCode,
            BrandId = brandId,
            RealName = playerCode,
            Password = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            WithdrawPassword = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            WithdrawSalt = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            Salt = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")),
            DefaultWalletCurrencyCode = "CNY",
            DefaultWalletId = Guid.NewGuid(),
            UpdatedOn = DateTime.Now,
            UpdatedByUsername = string.Empty,
        };
        await context.Players.AddRangeAsync(player).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);
        var actual = await fakeRepository.GetStatusAsync(playerCode).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(PlayerStatus.Enabled, actual);
    }

    [TestMethod]
    public async Task Get_Status_Is_Null()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new PlayerRepository(context);

        var playerCode = "4ucmqplxb029";

        var actual = await fakeRepository.GetStatusAsync(playerCode).ConfigureAwait(false);
        Assert.IsNull(actual);
    }
}
