using Tgs.GameCommand.Common;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class LicenseeRepository(TgsContext context) : ILicenseeRepository
{
	public ValueTask<LicenseeStatus> GetStatusAsync(
		Guid licenseeId,
		CancellationToken cancellationToken = default)
		=> context.Licensees
		.Where(x => x.Id == licenseeId)
		.Select(x => (LicenseeStatus)x.Status)
		.ToAsyncEnumerable()
		.FirstOrDefaultAsync(
			cancellationToken: cancellationToken);

	public IAsyncEnumerable<LicenseeInfo> QueryAsync(
		CancellationToken cancellationToken = default)
		=> (from licensee in context.Licensees.AsQueryable()
			join reseller in context.Resellers.AsQueryable()
			on licensee.ResellerId equals reseller.Id
			where licensee.Status != (int)LicenseeStatus.Archived
			select new LicenseeInfo(
				reseller.Id,
				reseller.Code,
				licensee.Id,
				licensee.Code))
		.ToAsyncEnumerable();
}
