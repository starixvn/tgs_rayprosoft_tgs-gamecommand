using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tgs.GameCommand.Repositories;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class SettingRepositoryUnitTest
{
	[TestMethod]
	public async Task Get_Configuration_Already_Exists()
	{
		var options = new DbContextOptionsBuilder<TgsContext>()
			.UseInMemoryDatabase(databaseName: "dbo")
			.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
			.Options;
		var context = new TgsContext(options);
		_ = context.Database.EnsureDeleted();

		var fakeRepository = new SettingRepository(context);
		var key = "KYConfiguration";
		var value = JsonSerializer.Serialize(new { CreateOn = DateTime.UtcNow });
		var setting = new Setting()
		{
			Key = key,
			Value = value,
			Description = string.Empty,
			CreatedOn = DateTime.UtcNow,
			UpdatedOn = DateTime.UtcNow,
			CreatedByUsername = string.Empty,
			UpdatedByUsername = string.Empty
		};

		await context.Settings.AddRangeAsync(setting).ConfigureAwait(false);
		_ = await context.SaveChangesAsync().ConfigureAwait(false);

		var actual = await fakeRepository.GetConfigurationAsync("kyconfiguration").ConfigureAwait(false);
		Assert.IsNotNull(actual);
		Assert.AreEqual(value, actual);
	}
}
