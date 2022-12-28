using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
    [TestMethod]
    public async Task Setting_Get_Configuration()
    {
        var key = "ky";

        var configuration = new TConfiguration(DateTimeOffset.UtcNow);

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<ISettingRepository>();

            _ = fakeRepository.GetConfigurationAsync(key)
                .Returns(ValueTask.FromResult<string?>(JsonSerializer.Serialize(configuration)));

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.SettingGetConfigurationAsync<TConfiguration>(key).ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(configuration.CreateOn, actual.CreateOn);
    }

    [TestMethod]
    public async Task Setting_Get_ConfigurationString()
    {
        var key = "ky";

        var configuration = new TConfiguration(DateTimeOffset.UtcNow);

        await using var web = CreateTestSettingSite(services =>
        {
            var fakeRepository = Substitute.For<ISettingRepository>();

            _ = fakeRepository.GetConfigurationAsync(key)
                .Returns(ValueTask.FromResult<string?>(JsonSerializer.Serialize(configuration)));

            _ = services.AddSingleton(fakeRepository);
        });

        var httpClient = web.CreateClient();

        var sut = new GameCommandClient(httpClient);

        var actual = await sut.SettingGetConfigurationStringAsync(key).ConfigureAwait(false);

        Assert.IsNotNull(actual);
    }
}
