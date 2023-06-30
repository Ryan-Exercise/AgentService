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

        public string GenerateDisableTableTrackingSQL(string tableName)
        {
            throw new NotImplementedException();
        }

        

        public string GenerateEnableDatabaseTrackingSQL(string database)
        {
            var builder = new StringBuilder();

            builder.AppendLine($@"IF NOT EXISTS(SELECT 1 FROM sys.change_tracking_databases WHERE database_id=DB_ID('{database}'))");
            builder.AppendLine("BEGIN");
            builder.AppendLine($@"ALTER DATABASE {database}");
            builder.AppendLine("SET CHANGE_TRACKING = ON(CHANGE_RETENTION = 7 DAYS, AUTO_CLEANUP = ON);");
            builder.AppendLine("END");

            return builder.ToString();
        }

        public string GenerateEnableTableTrackingSQL(string tableName)
        {
            throw new NotImplementedException();
        }

        public string GenerateQueryTableDeltaSQL(string table, params string[] parameters)
        {
            return $@"select c.SYS_CHANGE_OPERATION, c.SYS_CHANGE_VERSION, c.SYS_CHANGE_CREATION_VERSION,
";
        }
    }
}

