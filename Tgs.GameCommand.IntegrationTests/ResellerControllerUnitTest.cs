using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
    [TestMethod]
    public async Task Reseller_Get_Status()
    {
        var expected = ResellerStatus.Active;
        var resellerId = Guid.NewGuid();
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IResellerRepository>();

            _ = fakeRepository.GetStatusAsync(resellerId)
                .Returns(ResellerStatus.Active);

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.ResellerGetStatusAsync(resellerId).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(expected, actual);
    }
}
