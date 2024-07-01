using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;

namespace Tgs.GameCommandRepositories.UnitTest;

public class GameProviderStatusGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameProviderRepository = Substitute.For<IGameProviderRepository>();

		var query = new GameProviderStatusQuery(
			Code: "KY");

		var gameProviderId = Guid.NewGuid();
		_ = fakeGameProviderRepository
			.GetGameProviderIdAsync(
				code: Arg.Is(query.Code),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameProviderId);

		var status = GameProviderStatus.Active;
		_ = fakeGameProviderRepository
			.GetStatusAsync(
				id: Arg.Is(gameProviderId),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(status);

		var service = new GameProviderStatusGetService(
			fakeGameProviderRepository);

		var actual = await service.GetAsync(query);

		Assert.Equal(status, actual);
	}
}
