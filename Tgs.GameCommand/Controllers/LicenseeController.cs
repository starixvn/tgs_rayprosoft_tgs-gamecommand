using Microsoft.AspNetCore.Mvc;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Controllers;

[Route("[controller]")]
[ApiController]
public class LicenseeController : ControllerBase
{
	[HttpGet("{licenseeId}/status", Name = "Licensee_GetStatus")]
	public ValueTask<LicenseeStatus> GetStatusAsync(
		[FromServices] ILicenseeRepository repository,
		Guid licenseeId)
		=> repository.GetStatusAsync(
			licenseeId,
			cancellationToken: HttpContext.RequestAborted);

	[HttpGet(Name = "Licensee_Query")]
	public IAsyncEnumerable<LicenseeInfo> QueryAsync(
		[FromServices] ILicenseeRepository repository)
		=> repository.QueryAsync(
			cancellationToken: HttpContext.RequestAborted);
}
