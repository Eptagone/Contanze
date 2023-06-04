// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using Constanze.Core;
using Constanze.Core.Entities;
using Constanze.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;

namespace Constanze.AppService.Services;

/// <summary>
/// The Constanze bot service.
/// </summary>
public class ConstanzeBot : AsyncTelegramBotBase<ConstanzeInit>
{
	private readonly ConstanzeContext _context;
	private readonly ILogger<ConstanzeBot> _logger;
	private readonly SafoneGptService _gpt;

	private AppUser? _user;

	/// <summary>
	/// Initialize a new instance of the <see cref="ConstanzeBot"/> class.
	/// </summary>
	/// <param name="init"></param>
	public ConstanzeBot(ILogger<ConstanzeBot> logger, ConstanzeInit init, SafoneGptService safoneGptService, ConstanzeContext context) : base(init)
	{
		this._logger = logger;
		this._context = context;
		this._gpt = safoneGptService;
	}

	#region Message Handling

	protected override async Task OnMessageAsync(Message message, [Optional] CancellationToken cancellationToken)
	{
		// If the user is not available, do nothing.
		if (message.From == null)
		{
			return;
		}

		// Read the text from the message.
		var text = message.Text ?? message.Caption;

		// Get the user information and save it.
		var user = this.CreateUserIfNotExists(message.From);
		this._user = user;

		// If the message is empty, do nothing.
		if (string.IsNullOrEmpty(text))
		{
			// If the chat is a private chat or is a reply to a message sent by me, reply with a message.
			if (message.Chat.Type == ChatType.Private || message.ReplyToMessage?.From?.Username == this.Properties.User.Username)
			{
				this.Api.SendMessage(message.Chat.Id, "Sorry but i don't understand you.");
			}
		}
		else
		{
			BotCommandMatch botCommandMatch = this.Properties.CommandHelper.Match(text);
			// If the message is a bot command, process the command.
			if (botCommandMatch.Success)
			{
				await this.OnCommandAsync(message, botCommandMatch.Name, botCommandMatch.Params, cancellationToken);
			}
			// If the chat is a private chat, is a reply to a message sent by me or it has a mention to me, process the message.
			else if (message.Chat.Type == ChatType.Private || message.ReplyToMessage?.From?.Username == this.Properties.User.Username || text.Contains($"@{this.Properties.User.Username}"))
			{
				// Send a typing action to the chat.
				await this.Api.SendChatActionAsync(message.Chat.Id, ChatAction.Typing, cancellationToken: cancellationToken);

				// Recover the last active conversation for this chat and user.
				var conversation = this._context.Conversations
					.Include(d => d.DialogMessages)
					.Where(c => !c.IsClosed && c.ChatId == message.Chat.Id && c.User.Id == message.From.Id).FirstOrDefault();

				// If there is no active conversation, create a new one.
				if (conversation == null)
				{
					conversation = new Conversation
					{
						ChatId = message.Chat.Id,
					};
					// Add the conversation to the user.
					user.Conversations.Add(conversation);
				}

				// Create a new dialog message.
				var dialogMessage = new DialogMessage
				{
					// Set the message text sent by the user.
					User = text,
				};
				// Add the dialog message to the conversation.
				conversation.DialogMessages.Add(dialogMessage);

				// Send the message to GPT API.
				var response = await this._gpt.SendMessageAsync(text, conversation.DialogMessages, cancellationToken);

				// If the response is empty, reply with a message.
				if (response == null)
				{
					this.Api.SendMessage(message.Chat.Id, "Sorry but the gpt service is not available or your message was blocked.");
					return;
				}

				var botMessage = response.Message;

				// If the conversation has reached the maximum number of messages, close the conversation.
				if (conversation.DialogMessages.Count() + 1 >= this.Properties.MaxMessages)
				{
					conversation.IsClosed = true;
					botMessage += user.LanguageCode == "es"
						? "\n\nQuiero hablar de otra cosa. Olvidaré de qué estábamos hablando."
						: "\n\nI want to talk about something else. I'll forget what we were talking about.";
				}

				// Create a new dialog message.
				var responseMessage = new DialogMessage
				{
					// Set the message text sent by the GPT API.
					Bot = botMessage,
				};

				// Add the dialog message to the conversation.
				conversation.DialogMessages.Add(responseMessage);

				// Reply to the message.
				this.Api.SendMessage(message.Chat.Id, botMessage, replyToMessageId: message.MessageId, allowSendingWithoutReply: true);

				// Save the changes.
				await this._context.SaveChangesAsync(cancellationToken);
			}
		}
	}

	protected override async Task OnCommandAsync(Message message, string commandName, string commandParameters, [Optional] CancellationToken cancellationToken)
	{
		switch (commandName)
		{
			case "start":
				await this.Api.SendMessageAsync(message.Chat.Id, "Hello, I'm Constanze, a bot that uses the Safone ChatGPT API to talk with you.", cancellationToken: cancellationToken);
				break;

			case "reset":
				{
					await this.Api.SendMessageAsync(message.Chat.Id, "Conversation reseted.", cancellationToken: cancellationToken);

					// Recover the last active conversation for this chat and user.
					var conversation = this._context.Conversations
						.Include(d => d.DialogMessages)
						.Where(c => !c.IsClosed && c.ChatId == message.Chat.Id && c.User.Id == message.From!.Id).FirstOrDefault();

					// If there's an active conversation, close it.
					if (conversation != null)
					{
						conversation.IsClosed = true;
						await this._context.SaveChangesAsync(cancellationToken);
					}
				}
				break;

			case "help":
				{
					// Send a typing action to the chat.
					await this.Api.SendChatActionAsync(message.Chat.Id, ChatAction.Typing, cancellationToken: cancellationToken);

					var text = "I'm sorry but i can't help you.";

					// Recover the last active conversation for this chat and user.
					var conversation = this._context.Conversations
						.Include(d => d.DialogMessages)
						.Where(c => !c.IsClosed && c.ChatId == message.Chat.Id && c.User.Id == message.From!.Id).FirstOrDefault();

					// If there is no active conversation, create a new one.
					if (conversation == null)
					{
						conversation = new Conversation
						{
							ChatId = message.Chat.Id,
						};
						// Add the conversation to the user.
						this._user!.Conversations.Add(conversation);
					}

					// If the conversation has reached the maximum number of messages, close the conversation.
					if (conversation.DialogMessages.Count() + 1 >= this.Properties.MaxMessages)
					{
						conversation.IsClosed = true;
						text += this._user!.LanguageCode == "es"
							? "\n\nQuiero hablar de otra cosa. Olvidaré de qué estábamos hablando."
							: "\n\nI want to talk about something else. I'll forget what we were talking about.";
					}

					// Create a new dialog message.
					var dialogMessage = new DialogMessage
					{
						// Set the message text sent by the bot.
						Bot = text,
					};

					await this.Api.SendMessageAsync(message.Chat.Id, text, cancellationToken: cancellationToken);

					// Save the changes.
					await this._context.SaveChangesAsync(cancellationToken);
				}
				break;

			default:
				await this.Api.SendMessageAsync(message.Chat.Id, "Sorry but i don't understand you.", cancellationToken: cancellationToken);
				break;
		}
	}

	#endregion

	#region Exception Handling

	protected override async Task OnBotExceptionAsync(BotRequestException exp, [Optional] CancellationToken cancellationToken)
	{
		this._logger.LogError(exp, "An error occurred while a bot request was being processed.");
		await Task.CompletedTask;
	}


	protected override async Task OnExceptionAsync(Exception exp, [Optional] CancellationToken cancellationToken)
	{
		this._logger.LogError(exp, "An error occurred while processing the request.");
		await Task.CompletedTask;
	}

	#endregion

	/// <summary>
	/// Create a new user if not exists and return it.
	/// </summary>
	/// <param name="user">The telegram user.</param>
	/// <returns>The user.</returns>
	private AppUser CreateUserIfNotExists(User user)
	{
		// If user is already loaded, return it.
		if (this._user != null)
		{
			return this._user;
		}

		// Search the user in the database.
		var appUser = this._context.Users
			.Where(u => u.Id == user.Id).FirstOrDefault();

		// If the user does not exist, create a new one.
		if (appUser == null)
		{
			appUser = new AppUser
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Username = user.Username
			};
			this._context.Add(appUser);
			this._context.SaveChanges();
		}
		// If the user exists but their personal data has changed, update it and save it.
		else if (appUser.FirstName != user.FirstName || appUser.LastName != user.LastName || appUser.Username != user.Username)
		{
			appUser.FirstName = user.FirstName;
			appUser.LastName = user.LastName;
			appUser.Username = user.Username;

			this._context.SaveChanges();
		}

		return appUser;
	}
}
