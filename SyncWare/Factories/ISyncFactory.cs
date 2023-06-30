// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/SyncWare/Factories/IDbConnectionFactory.cs
// Author: 	ryanxu
// Email:	hitxcl@gmail.com
// Date Created: 29/06/2023
using System;
using System.Data;
using Sync.Core;
using Sync.Core.Script;

namespace Sync.Client.Service.Factories
{
    public delegate ISyncFactory SyncFactoryFactory(string identifier);

    public interface ISyncFactory
	{
        IDbConnection CreateConnection(string connectionString);
        IDbDataParameter CreateParameter(string paramKey, object paramValue);
        IScriptProvider CreateProvider(ScriptOptions options);
    }
}

