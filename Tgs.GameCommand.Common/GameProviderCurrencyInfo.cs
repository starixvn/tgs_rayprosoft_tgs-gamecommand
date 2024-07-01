namespace Tgs.GameCommand.Common;

public record GameProviderCurrencyInfo(
	Guid GameProviderId,
	string CurrencyCode,
	string GameProviderCurrencyCode);
