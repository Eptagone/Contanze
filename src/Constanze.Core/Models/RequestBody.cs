// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using Constanze.Core.Entities;
using System.Text.Json.Serialization;

namespace Constanze.Core.Models;

/// <summary>
/// Represents the request body of the message.
/// </summary>
/// <param name="Message">The new message sended by the user.</param>
/// <param name="DialogMessages">The previous dialog messages.</param>
public record RequestBody(
	[property: JsonPropertyName("message")] string Message,
	[property: JsonPropertyName("dialog_messages")] IEnumerable<DialogMessage> DialogMessages)
{
	[JsonPropertyName("chat_mode")]
	public string ChatMode { get; } = "assistant";
}
