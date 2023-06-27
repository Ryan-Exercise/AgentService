// // ${CopyrightHolder}
// // /Users/ryanxu/Documents/Projects/SyncWare/SyncWare/SyncService.cs
// // Author: 	ryanxu
// // Date Created: 23/06/2023
using System;
using Microsoft.Extensions.Options;
using Sync.Core;

namespace Demo.Console
{
	public class SyncService
	{
		private Synchronizer _synchronizer;

		public SyncService(IOptionsSnapshot<SyncConfig> config)
		{
			_synchronizer = new Synchronizer(config.Value);
            System.Console.WriteLine("Create SyncService");
        }

		public void Run() {
			System.Console.WriteLine("Sync Service is running");
			_synchronizer.Init();
		}
	}
}

