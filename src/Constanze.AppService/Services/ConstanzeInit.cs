// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using Constanze.Core;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace Constanze.AppService.Services;

/// <summary>
/// Defines the properties of the Constanze bot.
/// </summary>
public class ConstanzeInit : IBotProperties
{
	/// <summary>
	/// Initialize a new instance of the <see cref="ConstanzeInit"/> class.
	/// </summary>
	public ConstanzeInit(IConfiguration configuration)
	{
		var options = configuration.GetRequiredSection("Constanze").Get<ConstanzeOptions>()
			?? throw new ArgumentNullException(nameof(ConstanzeOptions), "The Constanze section is not configured.");

		this.Api = new(options.BotToken);
		this.User = this.Api.GetMe();
		this.CommandHelper = new BotCommandHelper(this);

		this.MaxMessages = options.MaxMessages;

		// Set my commands.
		this.Api.SetMyCommands(
			new BotCommand("reset", "Reset the conversation."),
			new BotCommand("help", "Show the help message.")
		);
	}

	public BotClient Api { get; }

	public User User { get; }

	public IBotCommandHelper CommandHelper { get; }

	public ushort MaxMessages { get; }
}
