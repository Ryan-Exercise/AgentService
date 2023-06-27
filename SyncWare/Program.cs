// Sync.Client.Service aims to synchronize data from source db to destination db on the basis of Change Data Tracking.

using System.Reflection;
using System.Text.Json;
using Demo.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sync.Core;



if (args.Count() != 1)
{
    throw new ArgumentNullException("Please check the argument of configuration file path");
}

var services = new ServiceCollection();
var configuration = new ConfigurationBuilder().AddJsonFile($"{args[0]}.config.json", optional: true, reloadOnChange: true).Build();
services.Configure<SyncConfig>(configuration);
services.AddScoped<SyncService>();

var provider = services.BuildServiceProvider();

using (var scope = provider.GetService<IServiceScopeFactory>()!.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<SyncService>();
    service.Run();
}
