// // ${CopyrightHolder}
// // /Users/ryanxu/Documents/Projects/SyncWare/SyncWare/SyncService.cs
// // Author: 	ryanxu
// // Date Created: 23/06/2023
using System;
using System.Data;
using System.Text;
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

            Console.WriteLine("Create SyncService");
        }

        private bool _isRunning;
        private Timer _timer = new Timer(10000);
		public void Run() {

            SeedDatabase();
            Console.WriteLine("Sync Service is running");

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

                    using (var command = connection.CreateCommand())
                    {
                        CommonEnhancer.Null(_origin.Options);
                        CommonEnhancer.Null(_origin.Options.Tables);
                        command.CommandType = CommandType.Text;
                        _origin.Options.Tables.ForEach(t =>
                        {
                            Console.WriteLine($"------------------- {t} begin -------------------");
                            List<string>? primaryKeys;
                            if (!_pkCache.TryGetValue(t, out primaryKeys))
                            {
                                primaryKeys = CommonEnhancer.GetTablePrimaryKeys(connection, t);
                                if (!_pkCache.TryAdd(t, primaryKeys))
                                {
                                    Console.WriteLine($"Failed to add {primaryKeys} as primary keys for {t} successfully");
                                }
                            }
                            command.CommandText = _provider.GenerateQueryTableDeltaSQL(t, primaryKeys.ToArray());
                            using (IDataReader reader = command.ExecuteReader())
                            {
                                
                                while (reader.Read())
                                {
                                    int rowNumber = 0;
                                    var rowBuilder = new StringBuilder($"{rowNumber++} \t");
                                    for (int i = 0;i < reader.FieldCount; i++)
                                    {
                                        rowBuilder.AppendFormat("{0}|\t", reader[i]);
                                    }
                                    rowBuilder.AppendLine();
                                    Console.WriteLine(rowBuilder.ToString());
                                }
                                //var schemaTable = reader.GetSchemaTable();
                                //CommonEnhancer.Null(schemaTable);
                                ////int rowNumber = 0;
                                //foreach (DataRow row in schemaTable.Rows)
                                //{
                                //    var rowBuilder = new StringBuilder($"{rowNumber++} \t");
                                //    foreach(DataColumn column in schemaTable.Columns)
                                //    {
                                //        rowBuilder.Append($"{column.ColumnName} = {row[column]},");
                                //    }
                                //    Console.WriteLine(rowBuilder.Remove(rowBuilder.Length - 1, 1).ToString());
                                //}
                            }
                            Console.WriteLine($"------------------- {t} end -------------------");
                        });
                        
                        
                        
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }


        private void SeedDatabase()
        {
            CommonEnhancer.Null(_origin.ConnectionString);
            using (var connection = _originFactory.CreateConnection(_origin.ConnectionString))
            {
                connection.Open();
                IDbCommand seedCommand = connection.CreateCommand();
                seedCommand.CommandText = File.ReadAllText("Seed.sql");
                seedCommand.ExecuteNonQuery();
                Console.WriteLine("Seed database completed!");
            }
        }

        private void ToggleSync(bool onOff)
        {
            CommonEnhancer.Null(_origin.ConnectionString);
            using (var connection = _originFactory.CreateConnection(_origin.ConnectionString))
            {
                connection.Open();
                CommonEnhancer.Null(_origin.Options);
                CommonEnhancer.Null(_origin.Options.Tables);
                var sqlBuilder = new StringBuilder();
                if(onOff)
                {
                    sqlBuilder.AppendLine(_provider.GenerateEnableDatabaseTrackingSQL(connection.Database));
                    _origin.Options.Tables.ForEach(t =>
                    {
                        sqlBuilder.AppendLine(_provider.GenerateEnableTableTrackingSQL(t));
                    });
                }
                else
                {
                    _origin.Options.Tables.ForEach(t =>
                    {
                        sqlBuilder.AppendLine(_provider.GenerateDisableTableTrackingSQL(t));
                    });
                    sqlBuilder.AppendLine(_provider.GenerateDisableDatabaseTrackingSQL(connection.Database));
                }
                
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlBuilder.ToString();
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                
            }
                
        }
	}
}

