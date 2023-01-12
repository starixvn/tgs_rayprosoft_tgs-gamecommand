using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core.ApplicationServices;

public record GameByGameProviderIdGameTypeQuery(
    Guid GameProviderId,
    GameType GameType);
