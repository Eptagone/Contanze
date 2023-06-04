// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using Constanze.AppService.Services;
using Constanze.Core;
using Constanze.Infrastructure;
using Constanze.Services;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((context, services) =>
	{
		var connectionString = context.Configuration.GetConnectionString("Default")
			?? throw new ArgumentNullException("ConnectionStrings:Default", "Connection string is null.");
		services.AddDbContext<ConstanzeContext>(options => options.UseSqlServer(connectionString));

		services.AddSingleton<SafoneGptService>();
		services.AddSingleton<ConstanzeInit>();
		services.AddScoped<ConstanzeBot>();

		services.AddHostedService<PollingService>();
	})
	.Build();

host.Run();
