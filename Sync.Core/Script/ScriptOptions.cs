// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/Sync.Core/Script/ScriptOptions.cs
// Author: 	ryanxu
// Email:	hitxcl@gmail.com
// Date Created: 29/06/2023
using System;
namespace Sync.Core.Script
{
	public class ScriptOptions
	{
        public List<string>? Tables { get; set; }
		public string? QueryTemplate { get; set; }
        public string? Extras { get; set; }
	}
}

