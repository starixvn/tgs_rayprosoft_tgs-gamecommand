namespace Tgs.GameCommand.Core;

public interface ISettingRepository
{
	ValueTask<string?> GetConfigurationAsync(string key);
}
