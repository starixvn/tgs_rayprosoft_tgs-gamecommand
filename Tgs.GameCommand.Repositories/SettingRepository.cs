using Microsoft.EntityFrameworkCore;
using Tgs.GameCommand.Core;
using Tgs.GameCommand.Repositories.Entities;

namespace Tgs.GameCommand.Repositories;

internal class SettingRepository(TgsContext context) : ISettingRepository
{
	public async ValueTask<string?> GetConfigurationAsync(string key)
#pragma warning disable CA1862 // 使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較
		=> (await context.Settings
		.AsQueryable()
		.FirstOrDefaultAsync(x => x.Key.ToLower() == key.ToLower())
		.ConfigureAwait(false))?.Value;

#pragma warning restore CA1862 // 使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較
}
