namespace Tgs.GameCommand.Common;

public record BrandGameProviderInfo(
      Guid BrandId,
      string BrandCode,
      Guid GameProviderId,
      string GameProviderCode,
      Guid GameProviderConfigurationId,
      string DefaultLanguageCode,
      string DesktopQueryString,
      string MobileQueryString);
