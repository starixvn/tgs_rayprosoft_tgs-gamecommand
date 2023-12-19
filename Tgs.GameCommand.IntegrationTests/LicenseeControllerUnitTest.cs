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
    public async Task Licensee_Get_Status()
    {
        var expected = LicenseeStatus.Active;
        var licenseeId = Guid.NewGuid();
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<ILicenseeRepository>();

            _ = fakeRepository.GetStatusAsync(licenseeId)
                .Returns(LicenseeStatus.Active);

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.LicenseeGetStatusAsync(licenseeId);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Licensee_Query()
    {
        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<ILicenseeRepository>();

            _ = fakeRepository
            .QueryAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(new LicenseeInfo[]
            {
                new(
                    Guid.NewGuid(),
                    "TGS",
                    Guid.NewGuid(),
                    "TGS"
                    )
            }.ToAsyncEnumerable());

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut
            .LicenseeQueryAsync()
            .ToArrayAsync()
            ?? Enumerable.Empty<LicenseeInfo>();

        Assert.True(actual.Any());
    }
}
