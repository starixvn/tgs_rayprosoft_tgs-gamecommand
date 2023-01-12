using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core.ApplicationServices;

public record GameByGameProviderCodeGameTypeQuery(
    string GameProviderCode,
    GameType GameType);
