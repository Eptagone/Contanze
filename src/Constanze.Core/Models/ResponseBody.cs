// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using System.Text.Json.Serialization;

namespace Constanze.Core.Models;

/// <summary>
/// Represents the response body.
/// </summary>
public record ResponseBody
{
	/// <summary>
	/// The size of the received message.
	/// </summary>
	[JsonPropertyName("input")]
	public ushort Input { get; set; }

	/// <summary>
	/// The response message.
	/// </summary>
	[JsonPropertyName("message")]
	public string Message { get; set; } = null!;

	/// <summary>
	/// The response mode.
	/// </summary>
	[JsonPropertyName("mode")]
	public string Mode { get; } = "assistant";

	/// <summary>
	/// The size of the response message.
	/// </summary>
	[JsonPropertyName("output")]
	public ushort Output { get; set; }

	/// <summary>
	/// The response time.
	/// </summary>
	[JsonPropertyName("time")]
	public string Time { get; set; } = null!;
}
