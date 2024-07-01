using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.IntegrationTests;

public partial class GameCommandUnitTest
{
	private static WebApplicationFactory<Program> CreateTestSettingSite(Action<IServiceCollection>? configureServices = null)
	=> new TestServiceApplication(hostBuilder =>
	{
		_ = hostBuilder
			.ConfigureAppConfiguration((_, configBuilder) =>
			{
				configBuilder.Sources.Clear();
			})
			.ConfigureServices(services =>
			{
				_ = services
					.AddSingleton(sp =>
					{
						var dbContextOptionsBuilder = new DbContextOptionsBuilder<TgsContext>();
						_ = dbContextOptionsBuilder
							.UseInMemoryDatabase(Guid.NewGuid().ToString())
							.ConfigureWarnings(x => x.Ignore(
								InMemoryEventId.TransactionIgnoredWarning));

						return new TgsContext(dbContextOptionsBuilder.Options);
					})
					.AddSingleton(sp =>
					{
						var dbContextOptionsBuilder = new DbContextOptionsBuilder<BetContext>();
						_ = dbContextOptionsBuilder
							.UseInMemoryDatabase(Guid.NewGuid().ToString())
							.ConfigureWarnings(x => x.Ignore(
								InMemoryEventId.TransactionIgnoredWarning));

						return new BetContext(dbContextOptionsBuilder.Options);
					}).AddSingleton<IConfigureOptions<OtlpExporterOptions>>(sp =>
					   new ConfigureNamedOptions<OtlpExporterOptions>(
						   Options.DefaultName,
						   exporterOptions =>
						   {
							   exporterOptions.TimeoutMilliseconds = 0;
						   })
						);

				configureServices?.Invoke(services);
			});
	});

	public record TConfiguration(DateTimeOffset CreateOn);
}
