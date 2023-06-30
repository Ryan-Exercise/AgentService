// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/SyncWare/Factories/InterbaseFactory.cs
// Author: 	ryanxu
// Email:	hitxcl@gmail.com
// Date Created: 28/06/2023
using System;
using System.Data;

using Sync.Core;
using Sync.Core.Script;

namespace Sync.Client.Service.Factories
{
	public class InterbaseFactory : ISyncFactory
    {
		public InterbaseFactory()
		{
            
		}

        public IDbConnection CreateConnection(string connectionString)
        {
            throw new NotImplementedException();
        }

        public IDbDataParameter CreateParameter(string paramKey, object paramValue)
        {
            throw new NotImplementedException();
        }

        public IScriptProvider CreateProvider(ScriptOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

