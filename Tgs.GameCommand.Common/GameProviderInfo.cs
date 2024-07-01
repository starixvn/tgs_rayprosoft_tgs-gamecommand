namespace Tgs.GameCommand.Common;

public record GameProviderInfo(
	Guid GameProviderId,
	string GameProviderCode,
	int UniqueId,
	string Name,
	bool TransferWallet,
	bool AutoCloseRound,
	bool SupportTestPlayer,
	bool AllowKickPlayer,
	short PlaceBetTimeout,
	short SettleBetTimeout,
	string? Description,
	string? Localization);
