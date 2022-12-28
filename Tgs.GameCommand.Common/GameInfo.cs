namespace Tgs.GameCommand.Common;

public record GameInfo(
    int GameId,
    string GameCode,
    Guid GameProviderId,
    string GameProviderCode,
    string GameExternalId,
    GameType Type,
    GamePlatformType PlatformType,
    string Name,
    bool SubGame,
    bool AvailableToBrand,
    bool HasTurnoverOnTieBet,
    bool Html5,
    string? Description,
    string? EndpointPath,
    string? RuleUrl,
    string[]? AvailableCurrencies,
    string? Localization,
    string? SubGameProviderCode);
