// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/SyncWare/ServiceEnhancer.cs
// Author: 	ryanxu
// Email:	hitxcl@gmail.com
// Date Created: 29/06/2023
using System;
using Sync.Core;
using System.Data;
using Sync.Client.Service.Factories;
using Sync.Core.Script;

namespace Sync.Client.Service
{
	public static class ServiceEnhancer
	{
        public static ISyncFactory FactoryReducer(EndPoint dbPoint, SyncFactoryFactory factory)
        {
            CommonEnhancer.Null(dbPoint.Server);
            var result = factory(dbPoint.Server);
            
            return factory(dbPoint.Server);
        }

        public static IDbConnection ConnectionReducer(EndPoint dbPoint, SyncFactoryFactory factory)
        {
            CommonEnhancer.Null(dbPoint.Server);
            CommonEnhancer.Null(dbPoint.ConnectionString);
            return factory(dbPoint.Server).CreateConnection(dbPoint.ConnectionString);
        }

        public static IScriptProvider ProviderReducer(EndPoint dbPoint, SyncFactoryFactory factory)
        {
            CommonEnhancer.Null(dbPoint.Server);
            CommonEnhancer.Null(dbPoint.Options);
            return factory(dbPoint.Server).CreateProvider(dbPoint.Options);
        }
    }
}

