using Microsoft.AspNetCore.Mvc;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;

namespace Tgs.GameCommand.Controllers;

[ApiController]
[Route("[controller]")]
public class BrandController : ControllerBase
{
    [HttpGet("{brandCode}/id", Name = "Brand_GetBrandId")]
    public ValueTask<Guid?> GetBrandIdAsync(
        [FromServices] IBrandRepository repository,
        string brandCode)
        => repository
        .GetBrandIdAsync(
            brandCode: brandCode,
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{brandId}/currency", Name = "Brand_GetCurrency")]
    public ValueTask<string?> GetCurrencyAsync(
        [FromServices] IBrandRepository repository,
        Guid brandId,
        [FromQuery] string currencyCode)
        => repository.GetCurrencyAsync(
            brandId: brandId,
            currencyCode: currencyCode,
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{brandId}/gp/configuration", Name = "Brand_GetGameProviderConfiguration")]
    public ValueTask<BrandGameProviderInfo?> GetGameProviderConfigurationAsync(
        [FromServices] IBrandRepository repository,
        Guid brandId,
        [FromQuery] Guid gameProviderId)
        => repository
        .GetGameProviderConfigurationAsync(
            brandId: brandId,
            gameProviderId: gameProviderId,
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{brandId}/gp/configuration/id", Name = "Brand_GetGameProviderConfigurationId")]
    public ValueTask<Guid?> GetGameProviderConfigurationIdAsync(
        [FromServices] IBrandRepository repository,
        Guid brandId,
        [FromQuery] Guid gameProviderId)
        => repository
        .GetGameProviderConfigurationIdAsync(
            brandId: brandId,
            gameProviderId: gameProviderId,
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{brandId}/gp/status", Name = "Brand_GetGameProviderStatus")]
    public ValueTask<GameProviderStatus?> GetGameProviderStatusAsync(
        [FromServices] IBrandRepository repository,
        Guid brandId,
        [FromQuery] Guid gameProviderId)
        => repository
        .GetGameProviderStatusAsync(
            brandId: brandId,
            gameProviderId: gameProviderId,
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{brandId}/game/status", Name = "Brand_GetGameStatus")]
    public ValueTask<GameStatus?> GetGameStatusAsync(
        [FromServices] IBrandRepository repository,
        Guid brandId,
        [FromQuery] int gameId)
        => repository
        .GetGameStatusAsync(
            brandId: brandId,
            gameId: gameId,
            cancellationToken: HttpContext.RequestAborted);

    [HttpGet("{brandId}/status", Name = "Brand_GetStatus")]
    public ValueTask<BrandStatus?> GetStatusAsync(
        [FromServices] IBrandRepository repository,
        Guid brandId)
        => repository.GetStatusAsync(
            brandId: brandId,
            cancellationToken: HttpContext.RequestAborted);
}
