using Microsoft.AspNetCore.Mvc;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Controllers;

[Route("[controller]")]
[ApiController]
public class ResellerController : ControllerBase
{
	[HttpGet("{resellerId}/status", Name = "Reseller_GetStatus")]
	public ValueTask<ResellerStatus> GetStatusAsync(
		[FromServices] IResellerRepository repository,
		Guid resellerId)
		=> repository.GetStatusAsync(
			resellerId: resellerId,
			cancellationToken: HttpContext.RequestAborted);

	[HttpGet(Name = "Reseller_Query")]
	public IAsyncEnumerable<ResellerInfo> QueryAsync(
		[FromServices] IResellerRepository repository)
		=> repository.QueryAsync(
			cancellationToken: HttpContext.RequestAborted);
}
