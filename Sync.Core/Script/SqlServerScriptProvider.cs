// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/Sync.Core/Script/SqlServerScriptProvider.cs
// Author: 	ryanxu
// Email:	hitxcl@gmail.com
// Date Created: 29/06/2023
using System;
using System.Reflection.Metadata;
using System.Text;

namespace Sync.Core.Script
{
	public class SqlServerScriptProvider : IScriptProvider
	{
		public SqlServerScriptProvider(ScriptOptions options)
		{
            Console.WriteLine("");
		}

        public string GenerateDisableDatabaseTrackingSQL(string database)
        {
            var builder = new StringBuilder();

            builder.AppendLine($@"IF EXISTS(SELECT 1 FROM sys.change_tracking_databases WHERE database_id=DB_ID('{database}'))");
            builder.AppendLine("BEGIN");
            builder.AppendLine($@"ALTER DATABASE {database}");
            builder.AppendLine("SET CHANGE_TRACKING = OFF;");
            builder.AppendLine("END");

            return builder.ToString();
        }

        public string GenerateDisableTablesTrackingSQL(IEnumerable<string> tableNames)
        {
            throw new NotImplementedException();
        }

        public string GenerateDisableTableTrackingSQL(string tableName)
        {
            var builder = new StringBuilder();
            builder.AppendLine($@"IF EXISTS(SELECT 1 FROM sys.change_tracking_tables WHERE OBJECT_ID=OBJECT_ID('{tableName}'))");
            builder.AppendLine($@"ALTER TABLE {tableName} DISABLE CHANGE_TRACKING;");
            return builder.ToString();
        }

        public string GenerateEnableDatabaseTrackingSQL(string database)
        {
            var builder = new StringBuilder();

            builder.AppendLine($@"IF NOT EXISTS(SELECT 1 FROM sys.change_tracking_databases WHERE database_id=DB_ID('{database}'))");
            builder.AppendLine("BEGIN");
            builder.AppendLine($@"ALTER DATABASE {database}");
            builder.AppendLine("SET CHANGE_TRACKING = ON(CHANGE_RETENTION = 4 DAYS, AUTO_CLEANUP = ON);");
            builder.AppendLine("END");

            return builder.ToString();
        }

        public string GenerateEnableTablesTrackingSQL(IEnumerable<string> tableNames)
        {
            throw new NotImplementedException();
        }

        public string GenerateEnableTableTrackingSQL(string tableName)
        {
            var builder = new StringBuilder();
            builder.AppendLine($@"IF NOT EXISTS(SELECT 1 FROM sys.change_tracking_tables WHERE OBJECT_ID=OBJECT_ID('{tableName}'))");
            builder.AppendLine($@"ALTER TABLE {tableName} ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);");
            return builder.ToString();
        }

        public string GenerateQueryTableDeltaSQL(string table, params string[] parameters)
        {
            
            var builder = new StringBuilder($@"SELECT t.*,c.* FROM CHANGETABLE(CHANGES {table}, 0) c ");
            builder.Append($@"LEFT JOIN {table} t ON ");
            builder.Append($@"{string.Join(" AND ", parameters.Select(p => $@"t.{p} = c.{p}"))} ");
            builder.Append("ORDER BY c.SYS_CHANGE_OPERATION;");
            return builder.ToString();
        }
    }
}

