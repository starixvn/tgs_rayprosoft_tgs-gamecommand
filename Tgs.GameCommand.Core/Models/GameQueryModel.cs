using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core.Models;

public record GameQueryModel(
    int GameId,
    string GameCode,
    Guid GameProviderId,
    string GameProviderCode,
    string GameExternalId,
    GameStatus Status,
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
