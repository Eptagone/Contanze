// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using System.Text.Json.Serialization;

namespace Constanze.Core.Entities;

/// <summary>
/// Represents the dialog message.
/// </summary>
public class DialogMessage
{
	/// <summary>
	/// The dialog message's unique identifier.
	/// </summary>
	[JsonIgnore]
	public int Id { get; set; }

	/// <summary>
	/// Represents the message sent by the user.
	/// </summary>
	[JsonPropertyName("user")]
	public string? User { get; init; }

	/// <summary>
	/// Represents the message sent by the bot.
	/// </summary>
	[JsonPropertyName("bot")]
	public string? Bot { get; init; }

	/// <summary>
	/// The conversation that owns the dialog message.
	/// </summary>
	[JsonIgnore]
	public Conversation Conversation { get; set; } = null!;
}
