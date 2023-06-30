// Sync.Client.Service aims to synchronize data from source db to destination db on the basis of Change Data Tracking.

using System.Reflection;
using System.Text.Json;
using Sync.Client.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sync.Client.Service.Factories;
using Sync.Core;


if (args.Count() != 1)
{
    throw new ArgumentNullException("Please check the argument of configuration file path");
}

var services = new ServiceCollection();
var configuration = new ConfigurationBuilder().AddJsonFile($"{args[0]}.config.json", optional: true, reloadOnChange: true).Build();
services.Configure<SyncConfig>(configuration);
services.AddScoped<SyncService>();
services.AddSingleton<ISyncFactory>(s => new SqlServerFactory());
services.AddSingleton<ISyncFactory>(s => new InterbaseFactory());
services.AddSingleton<SyncFactoryFactory>(provider => identifier =>
{
    var factories = provider.GetServices<ISyncFactory>().ToArray();
    return identifier switch
    {
        "MSSQL" => factories[0],
        "INTERBASE" => factories[1],
        _ => throw new InvalidOperationException()
    };
});

var provider = services.BuildServiceProvider();

using (var scope = provider.GetService<IServiceScopeFactory>()!.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<SyncService>();
    service.Run();
}
