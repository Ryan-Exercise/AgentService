// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/SyncWare/Factories/SqlServerFactory.cs
// Author: 	ryanxu
// Email:	hitxcl@gmail.com
// Date Created: 28/06/2023
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Sync.Core;
using Sync.Core.Script;

namespace Sync.Client.Service.Factories
{
	public class SqlServerFactory : ISyncFactory
    {
        
		public SqlServerFactory()
		{
            
		}

        public IDbConnection CreateConnection(string connectionString)
        {
            Console.WriteLine($"Create a SQLServer connection({connectionString})");
            return new SqlConnection(connectionString);
        }

        public IDbDataParameter CreateParameter(string paramKey, object paramValue)
        {
            return new SqlParameter(paramKey, paramValue.ToString());
        }

        public IScriptProvider CreateProvider(ScriptOptions options)
        {
            Console.WriteLine($"Create a SQLServer script provider()");
            return new SqlServerScriptProvider(options);
        }
    }
}

