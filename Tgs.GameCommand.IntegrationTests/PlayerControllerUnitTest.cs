using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
    [TestMethod]
    public async Task Player_Get()
    {
        var resellerId = Guid.NewGuid();
        var resellerCode = "TGS";
        var licenseeId = Guid.NewGuid();
        var licenseeCode = "TGS";
        var brandId = Guid.NewGuid();
        var brandCode = "GPI";
        var playerCode = "4ucmqplxb029";

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeService = Substitute.For<IGetService<PlayerQuery, PlayerInfo?>>();

            var query = new PlayerQuery(playerCode);
            _ = fakeService
            .GetAsync(
            query: query,
                cancellationToken: Arg.Any<CancellationToken>())
                .Returns(ValueTask.FromResult<PlayerInfo?>(
                new PlayerInfo(
                     resellerId,
                     resellerCode,
                     licenseeId,
                     licenseeCode,
                     brandId,
                     brandCode,
                     playerCode,
                     playerCode,
                     Guid.NewGuid(),
                     "CNY")));

            _ = services.AddSingleton(fakeService);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.PlayerGetAsync(playerCode).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(resellerId, actual.ResellerId);
        Assert.AreEqual(licenseeId, actual.LicenseeId);
        Assert.AreEqual(brandId, actual.BrandId);
        Assert.AreEqual(playerCode, actual.PlayerCode);
    }

    [TestMethod]
    public async Task Player_Get_Status()
    {
        var playerCode = "4ucmqplxb029";

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IPlayerRepository>();

            _ = fakeRepository.GetStatusAsync(playerCode)
                .Returns(ValueTask.FromResult<PlayerStatus?>(PlayerStatus.Enabled));

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.PlayerGetStatusAsync(playerCode).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(PlayerStatus.Enabled, actual);
    }
}
