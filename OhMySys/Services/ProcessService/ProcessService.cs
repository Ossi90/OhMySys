using OhMySys.Common.Helpers;
using OhMySys.Models.Records;
using System;
using System.Diagnostics;

namespace OhMySys.Services.ProcessService;

public class ProcessService : IProcessService
{
    public List<ProcessModel> GetActiveWindowProcesses()
    {
        try
        {
            var processes = Process
                .GetProcesses()
                .Where(p => p.ProcessName != null && !string.IsNullOrEmpty(p.MainWindowTitle) && IsAppProcess(p));

            List<ProcessModel> processModels = [];

            foreach (var process in processes)
            {
                var initialCpuTime = process.TotalProcessorTime;
                var initialTime = DateTime.UtcNow;

                Thread.Sleep(500);

                var finalCpuTime = process.TotalProcessorTime;
                var finalTime = DateTime.UtcNow;

                var cpuTimeUsed = finalCpuTime - initialCpuTime;
                var elapsedTime = finalTime - initialTime;
                var cpuUsage = (cpuTimeUsed.TotalMilliseconds / (elapsedTime.TotalMilliseconds * Environment.ProcessorCount)) * 100;

                var model = new ProcessModel
                {
                    Id = process.Id,
                    Name = process.ProcessName,
                    MainWindowTitle = process.MainWindowTitle,
                    Memory = process.GetProcessMemoryInMb(),
                    Description = process.GetDescription(),
                    InnerName = process.GetInnerName(),
                    Impact = process.GetProcessImpact(),
                    CpuUsage = cpuUsage,
                    Status = process.GetStatus(),
                    Icon = process.GetIcon(),

                    Path = process.GetPath(),
                };

                processModels.Add(model);
            }

            return processModels;
        }
        catch (Exception e)
        {
            return [];
        }
    }

    private static string GetName(Process process)
    {
        return !process.CanAccessMainModule()
            ? process.MainWindowTitle
            : process.MainModule?.FileVersionInfo?.FileDescription ?? process.MainModule?.FileVersionInfo?.InternalName ?? process.ProcessName;
    }

    private bool IsAppProcess(Process process)
    {
        try
        {
            var module = process.GetProcessMainModule();

            if (module == null || string.IsNullOrEmpty(module?.FileName))
            {
                return false;
            }

            return !module.FileName.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.Windows), StringComparison.OrdinalIgnoreCase) && process.Id != 0;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}