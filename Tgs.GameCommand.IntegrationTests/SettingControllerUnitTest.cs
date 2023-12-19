using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameCommand.Client;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
    [Fact]
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

        var actual = await sut.SettingGetConfigurationAsync<TConfiguration>(key);

        Assert.NotNull(actual);
        Assert.Equal(configuration.CreateOn, actual.CreateOn);
    }

    [Fact]
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

        var actual = await sut.SettingGetConfigurationStringAsync(key);

        Assert.NotNull(actual);
    }
}
