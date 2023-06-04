// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using Constanze.Core.Entities;
using Constanze.Core.Models;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Constanze.Core;

/// <summary>
/// Client for the Safone GPT API.
/// </summary>
public class SafoneGptService
{
	private static JsonSerializerOptions SerializerOptions = new()
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
	};

	private static readonly HttpClient HttpClient = new()
	{
		BaseAddress = new("https://api.safone.me")
	};

	#region Fields
	private readonly ILogger<SafoneGptService> _logger;
	#endregion

	/// <summary>
	/// Initialize a new instance of the <see cref="SafoneGptService"/> class.
	/// </summary>
	public SafoneGptService(ILogger<SafoneGptService> logger)
	{
		this._logger = logger;
	}

	/// <summary>
	/// Send a message to the Safone GPT API.
	/// </summary>
	/// <param name="message">The message to send.</param>
	/// <param name="dialogs">A list of all previous messages.</param>
	/// <returns>The response body.</returns>
	public ResponseBody? SendMessage(string message, [Optional] IEnumerable<DialogMessage>? dialogs)
	{
		return this.SendMessageAsync(message, dialogs).Result;
	}

	/// <summary>
	/// Send a message to the Safone GPT API.
	/// </summary>
	/// <param name="message">The message to send.</param>
	/// <param name="dialogs">A list of all previous messages.</param>
	/// <returns>The response body.</returns>
	public async Task<ResponseBody?> SendMessageAsync(string message, [Optional] IEnumerable<DialogMessage>? dialogs, [Optional] CancellationToken? cancellationToken)
	{
		// Initialize the request body.
		RequestBody body = new(message, dialogs ?? Array.Empty<DialogMessage>());
		// Send the request.
		var response = HttpClient.PostAsJsonAsync("/chatgpt", body, SerializerOptions, cancellationToken ?? default);

		// Check if the response is successful.
		if (response.Result.IsSuccessStatusCode)
		{
			// Get the response body.
			var responseBody = await response.Result.Content.ReadFromJsonAsync<ResponseBody>();
			// Check if the response body is not null.
			if (responseBody is not null)
			{
				// Return the response body.
				return responseBody;
			}
			else
			{
				// Log the error.
				this._logger.LogError("Error: The response body is null.");
			}
		}
		else
		{
			// Log the error.
			this._logger.LogError("Error: {StatusCode} - {ReasonPhrase}", response.Result.StatusCode, response.Result.ReasonPhrase);
		}

		// Return null.
		return null;
	}
}
