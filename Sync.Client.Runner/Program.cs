// Sync.Client.Runner aims to run individual process for every single Sync.Clent.Service 

using System.Diagnostics;
using System.Linq;
using Sync.Client.Runner;

var config = ServiceConfig.Default();
string assemblyPath = AppContext.BaseDirectory;
string commandPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(assemblyPath, config.LaunchPath!));

Console.WriteLine($"Command path is {commandPath}");

string command = Path.Combine(commandPath, "Sync.Client.Service.dll");
var processes = new List<Process>();
config.Nodes!.ForEach(n =>
{
    //string node = Path.Combine(commandPath,n);
    Process process = new Process();
    process.StartInfo.FileName = "dotnet";
    process.StartInfo.Arguments = $"Sync.Client.Service.dll {n}";
    process.StartInfo.WorkingDirectory = commandPath;
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.RedirectStandardOutput = true;
    process.StartInfo.RedirectStandardError = true;

    try
    {
        process.Start();
        Console.WriteLine($"Waiting for process {n} running out");

        process.OutputDataReceived += new DataReceivedEventHandler(
    (s, e) =>
    {
        //do something with the output data 'e.Data'
        Console.WriteLine("O: " + e.Data);
    }
);
        process.ErrorDataReceived += new DataReceivedEventHandler(
            (s, e) =>
            {
                //do something with the error data 'e.Data'
                Console.WriteLine("E: " + e.Data);
            }
        );
        //start process
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        processes.Add(process);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred: " + ex.Message);
    }
});

var tasks = processes.Select(p => p.WaitForExitAsync());
await Task.WhenAll(tasks);

Console.WriteLine("All processes completed");
Console.ReadLine();
