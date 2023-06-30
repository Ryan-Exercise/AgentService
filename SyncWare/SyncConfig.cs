using System.IO;
using System.Reflection;
using System.Text.Json;
using Sync.Core.Script;

namespace Sync.Core;
public class SyncConfig
{
    public EndPoint? Origin { get; set; }
    public EndPoint? Destination { get; set; }
    

    public static SyncConfig Default() {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "./";
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        try {
            var config = JsonSerializer.Deserialize<SyncConfig>(File.ReadAllText(Path.Combine(path, "config.json")), options);
            return config!;
        }
        catch
        {
            throw;
        }
    }
}

public class EndPoint
{
    public string? Server { get; set; }
    public string? ConnectionString { get; set; }
    public ScriptOptions? Options { get; set; }
}


