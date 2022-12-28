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
public class ResellerRepositoryUnitTest
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

        var fakeRepository = new ResellerRepository(context);
        var resellerId = Guid.NewGuid();
        var reseller = new Reseller()
        {
            Id = resellerId,
            Status = (short)ResellerStatus.Archived,
            Description = string.Empty,
            OperationalEmail = string.Empty,
            Code = "GPI",
            Name = "GPI",
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            CreatedByUsername = string.Empty,
            UpdatedByUsername = string.Empty,
        };

        await context.Resellers.AddRangeAsync(reseller).ConfigureAwait(false);
        _ = await context.SaveChangesAsync().ConfigureAwait(false);

        var actual = await fakeRepository.GetStatusAsync(resellerId).ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(ResellerStatus.Archived, actual);
    }
}
