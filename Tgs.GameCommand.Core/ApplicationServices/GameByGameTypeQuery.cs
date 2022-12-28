using Tgs.GameCommand.Common;

namespace Tgs.GameCommand.Core.ApplicationServices;

public record GameByGameTypeQuery(
    string GameProviderCode,
    GameType GameType);
