// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/Syncor/Synchronizer.cs
// Author: 	ryanxu
// Date Created: 22/06/2023
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Sync.Core
{

	public class Synchronizer
	{
		private SyncConfig _config;
		public Synchronizer(SyncConfig? config = default)
		{
			_config = config ?? SyncConfig.Default();
		}

		public void Init()
		{
			Console.WriteLine($"Origin db is {_config.Origin?.ConnectionString}");
		}
	}
}


