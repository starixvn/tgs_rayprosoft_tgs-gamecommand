using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tgs.GameCommand.IntegrationTests;

internal class TestServiceApplication : WebApplicationFactory<Program>
{
    private readonly Action<IWebHostBuilder>? _webHostConfigure;

    public TestServiceApplication(Action<IWebHostBuilder>? webHostConfigure = null)
    {
        _webHostConfigure = webHostConfigure;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => _webHostConfigure?.Invoke(builder);
}
