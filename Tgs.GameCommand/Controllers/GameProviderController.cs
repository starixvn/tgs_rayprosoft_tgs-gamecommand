using Microsoft.AspNetCore.Mvc;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameCommand.Controllers;

[Route("[controller]")]
[ApiController]
public class GameProviderController : ControllerBase
{
    [HttpGet("{id}", Name = "GameProvider_Get")]
    public ValueTask<GameProviderInfo?> GameProviderGetAsync(
        [FromServices] IGetService<GameProviderQuery, GameProviderInfo?> service,
        Guid id)
        => service
        .GetAsync(
            query: new GameProviderQuery(id),
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("query/id", Name = "GameProvider_Get_GameProviderIds")]
    public async IAsyncEnumerable<GameProviderIdInfo> GameProviderGetGameProviderIdsAsync(
        [FromServices] IGameProviderRepository repository)
    {
        var gameProviderIdInfos = await repository
            .GetGameProviderIdsAsync(
                HttpContext.RequestAborted)
            .ToArrayAsync()
            .ConfigureAwait(false);

        if (!gameProviderIdInfos.Any())
        {
            yield break;
        }

        foreach (var gameProviderIdInfo in gameProviderIdInfos)
        {
            yield return new Common.GameProviderIdInfo(
                gameProviderIdInfo.GameProviderCode,
                gameProviderIdInfo.GameProviderId);
        }
    }

    [HttpGet("{id}/culturecode", Name = "GameProvider_GetCultureCode")]
    public ValueTask<string?> GetCultureCodeAsync(
        [FromServices] IGameProviderRepository repository,
        Guid id,
        [FromQuery] string cultureCode)
        => repository
        .GetCultureCodeAsync(
            id,
            cultureCode,
            HttpContext.RequestAborted);

    [HttpGet("{id}/currency", Name = "GameProvider_GetCurrency")]
    public async ValueTask<GameProviderCurrencyInfo?> GetCurrencyAsync(
        [FromServices] IGameProviderRepository repository,
        Guid id,
        [FromQuery] string currencyCode)
    {
        var data = await repository
            .GetCurrencyAsync(
                id,
                currencyCode,
                HttpContext.RequestAborted).ConfigureAwait(false);

        return data != null
            ? new Common.GameProviderCurrencyInfo(
                data.GameProviderId,
                data.CurrencyCode,
                data.GameProviderCurrencyCode)
            : null;
    }

    [HttpGet("{id}/currencycode", Name = "GameProvider_GetCurrencyCode")]
    public ValueTask<string?> GetCurrencyCodeAsync(
        [FromServices] IGameProviderRepository repository,
        Guid id,
        [FromQuery] string currencyCode)
        => repository
        .GetCurrencyCodeAsync(
            id,
            currencyCode,
            HttpContext.RequestAborted);

    [HttpGet("{code}/id", Name = "GameProvider_GetGameProviderId")]
    public ValueTask<Guid?> GetGameProviderIdAsync(
        [FromServices] IGameProviderRepository repository,
        string code)
        => repository
        .GetGameProviderIdAsync(
            code,
            HttpContext.RequestAborted);

    [HttpGet("{id}/status", Name = "GameProvider_GetStatus")]
    public ValueTask<GameProviderStatus> GetStatusAsync(
        [FromServices] IGameProviderRepository repository,
        Guid id)
        => repository
        .GetStatusAsync(
            id,
            HttpContext.RequestAborted);
}
