using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tgs.GameCommand.Common;
using Tgs.GameCommand.Repositories;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommandRepositories.UnitTest;

[TestClass]
public class LicenseeRepositoryUnitTest
{
    [TestMethod]
    public async Task Get_Status()
    {
        var options = new DbContextOptionsBuilder<TgsContext>()
            .UseInMemoryDatabase(databaseName: "dbo")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var context = new TgsContext(options);
        _ = context.Database.EnsureDeleted();

        var fakeRepository = new LicenseeRepository(context);
        var resellerId = Guid.NewGuid();
        var licenseeId = Guid.NewGuid();
        var licensee = new Licensee()
        {
            Id = licenseeId,
            Status = (short)LicenseeStatus.Archived,
            Description = string.Empty,
            OperationalEmail = string.Empty,
            ResellerId = resellerId,
            Code = "GPI",
            Name = "GPI",
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
        };

        await context.Licensees.AddRangeAsync(licensee).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.GetStatusAsync(licenseeId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(LicenseeStatus.Archived, actual);
    }
}
