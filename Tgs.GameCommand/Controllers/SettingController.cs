using Microsoft.AspNetCore.Mvc;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingController : ControllerBase
{
	[HttpGet("{key}/value", Name = "Setting_GetConfiguration")]
	public ValueTask<string?> GetConfigurationAsync(
		[FromServices] ISettingRepository roundRepository,
		string key)
		=> roundRepository.GetConfigurationAsync(key);
}
