// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Constanze.Core.Entities;

/// <summary>
/// Represents a conversation of a user with the bot.
/// </summary>
public class Conversation
{
	/// <summary>
	/// Initialize a new instance of <see cref="Conversation"/>.
	/// </summary>
	public Conversation()
	{
		this.DialogMessages = new HashSet<DialogMessage>();
	}

	/// <summary>
	/// The conversation's unique identifier.
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// True if the conversation is closed.
	/// </summary>
	public bool IsClosed { get; set; }

	/// <summary>
	/// The unique identifier for this chat.
	/// </summary>
	[Required]
	public long ChatId { get; set; }

	/// <summary>
	/// The user who owns the conversation.
	/// </summary>
	public AppUser User { get; set; } = null!;

	/// <summary>
	/// List of messages sent by the user and the bot.
	/// </summary>
	[InverseProperty("Conversation")]
	public ICollection<DialogMessage> DialogMessages { get; }
}
