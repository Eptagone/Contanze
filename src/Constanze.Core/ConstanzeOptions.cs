// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

namespace Constanze.Core;

/// <summary>
/// Configuration options for Constanze bot.
/// </summary>
public record ConstanzeOptions
{
	/// <summary>
	/// The bot token used to connect to the Telegram Bot API.
	/// </summary>
	public string BotToken { get; init; } = null!;

	/// <summary>
	/// The maximum number of messages that can be sent in a conversation.
	/// </summary>
	public ushort MaxMessages { get; init; } = 20;
}
