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

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Reseller_Query()
    {
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<IResellerRepository>();

            _ = fakeRepository
            .QueryAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(new ResellerInfo[]
            {
                new(
                    Guid.NewGuid(),
                    "TGS"
                    )
            }.ToAsyncEnumerable());

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .ResellerQueryAsync()
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<ResellerInfo>();

        Assert.True(actual.Any());
    }
}
