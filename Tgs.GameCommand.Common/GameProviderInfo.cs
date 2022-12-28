namespace Tgs.GameCommand.Common;

public record GameProviderInfo(
    Guid GameProviderId,
    string? Description,
    bool AutoCloseRound,
    bool SupportTestPlayer,
    bool AllowKickPlayer,
    short PlaceBetTimeout,
    short SettleBetTimeout,
    int UniqueId,
    bool TransferWallet,
    string GameProviderCode,
    string Name,
    string? Localization);
