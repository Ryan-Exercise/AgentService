// // ${CopyrightHolder}
// // /Users/ryanxu/Documents/Projects/SyncWare/SyncWare/SyncService.cs
// // Author: 	ryanxu
// // Date Created: 23/06/2023
using System;
using System.Data;
using System.Timers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Sync.Client.Service;
using Sync.Client.Service.Factories;
using Sync.Core;
using Sync.Core.Script;
using Timer = System.Timers.Timer;

namespace Sync.Client.Service
{
	public class SyncService
	{
		private IScriptProvider _provider;
        private ISyncFactory _originFactory;
        private EndPoint _origin;
        private Dictionary<string, List<string>> _pkCache = new Dictionary<string, List<string>>();
        private Dictionary<string, int> _trackingVersions = new Dictionary<string, int>();

		public SyncService(IOptionsSnapshot<SyncConfig> config, SyncFactoryFactory factory)
		{
			var configValue = config.Value;
            CommonEnhancer.Null(configValue.Origin);
            _origin = configValue.Origin;
            _originFactory = ServiceEnhancer.FactoryReducer(configValue.Origin, factory);
            
            CommonEnhancer.Null(configValue.Destination);
            var destinationConnection = ServiceEnhancer.ConnectionReducer(configValue.Destination, factory);

            _provider = ServiceEnhancer.ProviderReducer(configValue.Origin, factory);

            System.Console.WriteLine("Create SyncService");
        }

        private bool _isRunning;
        private Timer _timer = new Timer(10000);
		public void Run() {
            

            System.Console.WriteLine("Sync Service is running");
            ToggleSync(true);
            if (!_isRunning)
            {
                _timer.Elapsed += DoSync;
                _timer.AutoReset = true;
                _timer.Enabled = true;
                _timer.Start();
                _isRunning = true;
                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
                _timer.Stop();
                _isRunning = false;
            }
            ToggleSync(false);
        }

        private void DoSync(object? sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Running task...");
            CommonEnhancer.Null(_origin.ConnectionString);
            using(var connection = _originFactory.CreateConnection(_origin.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var tableName = "Consumer";
                    List<string>? primaryKeys;
                    if (!_pkCache.TryGetValue(tableName, out primaryKeys))
                    {
                        primaryKeys = CommonEnhancer.GetTablePrimaryKeys(connection, tableName);
                        if(_pkCache.TryAdd(tableName, primaryKeys))
                        {
                            Console.WriteLine($"Add {primaryKeys} as primary keys for {tableName} successfully");
                        }
                    }
                    

                    //using(var command = connection.CreateCommand())
                    //{
                    //    command.CommandText = _provider.GenerateDeltaQuery("Consumer");
                    //    command.CommandType = CommandType.Text;
                    //    using(IDataReader reader = command.ExecuteReader())
                    //    {
                    //        while (reader.Read())
                    //        {
                    //            Console.WriteLine($"{reader.GetValue(0)},{reader.GetValue(1)}");
                    //        }
                            
                    //    }
                    //}
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }

        private void ToggleSync(bool onOff)
        {
            CommonEnhancer.Null(_origin.ConnectionString);
            using (var connection = _originFactory.CreateConnection(_origin.ConnectionString))
            {
                CommonEnhancer.Null(_origin.Options);

                var sql = onOff ? _provider.GenerateEnableDatabaseTrackingSQL(connection.Database)
                    : _provider.GenerateDisableDatabaseTrackingSQL(connection.Database);

                connection.Open();
                
                IDbCommand command = connection.CreateCommand();
                
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                Console.WriteLine(command.ExecuteNonQuery());
            }
                
        }

       
	}
}

