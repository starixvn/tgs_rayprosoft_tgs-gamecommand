namespace Tgs.GameCommand.Core.Models;

public record GameProviderCurrencyModel(
    Guid GameProviderId,
    string CurrencyCode,
    string GameProviderCurrencyCode);
