// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1862:使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較", Justification = "<暫止>", Scope = "member", Target = "~M:Tgs.GameCommand.Repositories.GameProviderConfigurationRepository.QueryAsync(System.String,System.Threading.CancellationToken)~System.Collections.Generic.IAsyncEnumerable{Tgs.GameCommand.Common.GameProviderConfigInfo}")]
[assembly: SuppressMessage("Performance", "CA1862:使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較", Justification = "<暫止>", Scope = "member", Target = "~M:Tgs.GameCommand.Repositories.GameProviderRepository.GetCultureCodeAsync(System.Guid,System.String,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask{System.String}")]
[assembly: SuppressMessage("Performance", "CA1862:使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較", Justification = "<暫止>", Scope = "member", Target = "~M:Tgs.GameCommand.Repositories.GameProviderRepository.GetCurrencyAsync(System.Guid,System.String,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask{Tgs.GameCommand.Core.Models.GameProviderCurrencyModel}")]
[assembly: SuppressMessage("Performance", "CA1862:使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較", Justification = "<暫止>", Scope = "member", Target = "~M:Tgs.GameCommand.Repositories.GameProviderRepository.GetGameProviderIdAsync(System.String,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask{System.Nullable{System.Guid}}")]
[assembly: SuppressMessage("Performance", "CA1862:使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較", Justification = "<暫止>", Scope = "member", Target = "~M:Tgs.GameCommand.Repositories.GameProviderRepository.GetStatusAsync(System.String,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask{System.Nullable{Tgs.GameCommand.Common.GameProviderStatus}}")]
[assembly: SuppressMessage("Performance", "CA1862:使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較", Justification = "<暫止>", Scope = "member", Target = "~M:Tgs.GameCommand.Repositories.PlayerRepository.GetAsync(System.String,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask{Tgs.GameCommand.Common.PlayerInfo}")]
[assembly: SuppressMessage("Performance", "CA1862:使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較", Justification = "<暫止>", Scope = "member", Target = "~M:Tgs.GameCommand.Repositories.PlayerRepository.GetStatusAsync(System.String,System.Threading.CancellationToken)~System.Threading.Tasks.ValueTask{System.Nullable{Tgs.GameCommand.Common.PlayerStatus}}")]
