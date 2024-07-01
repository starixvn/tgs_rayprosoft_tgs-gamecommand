namespace Tgs.GameCommand.Common;

public record GameProviderConfigInfo(
	Guid GameProviderId,
	string GameProviderCode,
	Guid GameProviderConfigId,
	GameProviderConfigStatus Status,
	string Name,
	string? Description,
	string ClientId,
	string SecretKey,
	string DesktopEndpoint,
	string MobileEndpoint,
	string ConfigurationData,
	bool IsDefault);
