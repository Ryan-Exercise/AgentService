// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/Sync.Core/Script/IScriptProvider.cs
// Author: 	ryanxu
// Email:	hitxcl@gmail.com
// Date Created: 29/06/2023
using System;
namespace Sync.Core.Script
{
	public interface IScriptProvider
	{
		string GenerateEnableDatabaseTrackingSQL(string database);
		string GenerateDisableDatabaseTrackingSQL(string database);
		string GenerateEnableTableTrackingSQL(string tableName);
        string GenerateDisableTableTrackingSQL(string tableName);
        string GenerateQueryTableDeltaSQL(string table, params string[] parameters);
	}
}

