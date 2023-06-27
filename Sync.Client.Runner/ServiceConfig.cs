// ${CopyrightHolder}
// /Users/ryanxu/Documents/Projects/SyncWare/Sync.Client.Runner/ServiceConfig.cs
// Author: 	ryanxu
// Email:	hitxcl@gmail.com
// Date Created: 27/06/2023
using System;
using System.Reflection;
using System.Text.Json;

namespace Sync.Client.Runner
{
	public class ServiceConfig
	{
        public string? LaunchPath { get; set; }
        public List<string>? Nodes { get; set; }
		public ServiceConfig()
		{
		}

        public static ServiceConfig Default()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "./";
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            try
            {
                var config = JsonSerializer.Deserialize<ServiceConfig>(File.ReadAllText(Path.Combine(path, "service.json")), options);
                return config!;
            }
            catch
            {
                throw;
            }
        }
    }
}

