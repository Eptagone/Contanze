// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using Constanze.AppService.Services;
using Constanze.Infrastructure;
using Telegram.BotAPI.GettingUpdates;

namespace Constanze.Services;

/// <summary>
/// The long-running service that polls the Telegram Bot API for updates.
/// </summary>
public class PollingService : BackgroundService
{
	private readonly ILogger<PollingService> _logger;
	private readonly ConstanzeInit _properties;
	private readonly IServiceProvider _serviceProvider;

	/// <summary>
	/// Intializes a new instance of <see cref="PollingService"/>.
	/// </summary>
	/// <param name="logger">The logger.</param>
	/// <param name="bot">The bot service.</param>
	public PollingService(ILogger<PollingService> logger, ConstanzeInit init, IServiceProvider serviceProvider)
	{
		this._logger = logger;
		this._properties = init;
		this._serviceProvider = serviceProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		this._logger.LogInformation("Polling Service running at: {time}", DateTimeOffset.Now);

		// Ensure the database is created.
		using (var scope = this._serviceProvider.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ConstanzeContext>();
			await context.Database.EnsureCreatedAsync(stoppingToken).ConfigureAwait(false);
		}

		// Get Updates for the first time.
		var updates = await this._properties.Api.GetUpdatesAsync(allowedUpdates: Array.Empty<string>(), cancellationToken: stoppingToken).ConfigureAwait(false);

		while (!stoppingToken.IsCancellationRequested)
		{
			// Process updates if any updates are available.
			if (updates.Any())
			{
				/// Process updates in parallel.
				await Parallel.ForEachAsync(updates, stoppingToken, async (update, cancellationToken) => await this.ProcessUpdate(update, cancellationToken)).ConfigureAwait(false);

				// Get updates from the last update id + 1.
				updates = await this._properties.Api.GetUpdatesAsync(updates[^1].UpdateId + 1, cancellationToken: stoppingToken).ConfigureAwait(false);
			}
			// Get updates if no updates are available.
			else
			{
				updates = await this._properties.Api.GetUpdatesAsync(cancellationToken: stoppingToken).ConfigureAwait(false);
			}
		}
	}

	/// <summary>
	/// Stop the polling service.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	public override Task StopAsync(CancellationToken cancellationToken)
	{
		this._logger.LogInformation("Polling Service stopping at: {time}", DateTimeOffset.Now);
		return base.StopAsync(cancellationToken);
	}

	/// <summary>
	/// Process a new update from Telegram.
	/// </summary>
	/// <param name="update">The update to process.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	private async Task ProcessUpdate(Update update, CancellationToken cancellationToken)
	{
		using var scope = this._serviceProvider.CreateScope();
		var bot = scope.ServiceProvider.GetRequiredService<ConstanzeBot>();
		await bot.OnUpdateAsync(update, cancellationToken: cancellationToken).ConfigureAwait(false);
	}
}
