// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/Sync.Core/Script/InterbaseScriptProvider.cs
// Author: 	ryanxu
// Email:	hitxcl@gmail.com
// Date Created: 29/06/2023
using System;

namespace Sync.Core.Script
{
	public class InterbaseScriptProvider : IScriptProvider
	{
		public InterbaseScriptProvider(ScriptOptions options)
		{
		}

        public string GenerateDisableDatabaseTrackingSQL(string database)
        {
            throw new NotImplementedException();
        }

        public string GenerateDisableTablesTrackingSQL(IEnumerable<string> tableNames)
        {
            throw new NotImplementedException();
        }

        public string GenerateDisableTableTrackingSQL(string tableName)
        {
            throw new NotImplementedException();
        }

        public string GenerateEnableDatabaseTrackingSQL(string database)
        {
            throw new NotImplementedException();
        }

        public string GenerateEnableTablesTrackingSQL(IEnumerable<string> tableNames)
        {
            throw new NotImplementedException();
        }

        public string GenerateEnableTableTrackingSQL(string tableName)
        {
            throw new NotImplementedException();
        }

        public string GenerateQueryTableDeltaSQL(string table, params string[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}

