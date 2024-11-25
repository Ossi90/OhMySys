using Microsoft.VisualBasic.Devices;
using OpenHardwareMonitor.Hardware;
using System.IO;
using System.Management;

namespace OhMySys.Services.CurrentMachineStatus;

public class CurrentMachineStatusService : ICurrentMachineStatusService
{
    public double GetUsedMemoryInGB()
    {
        var computerInfo = new ComputerInfo();
        var totalPhysicalMemory = computerInfo.TotalPhysicalMemory;
        var availablePhysicalMemory = computerInfo.AvailablePhysicalMemory;
        var usedMemory = totalPhysicalMemory - availablePhysicalMemory;

        return ConvertBytesToGigabytes(usedMemory);
    }

    public double GetTotalMemoryInGB()
    {
        var computerInfo = new ComputerInfo();
        var totalPhysicalMemory = computerInfo.TotalPhysicalMemory;

        return ConvertBytesToGigabytes(totalPhysicalMemory);
    }

    public double GetCurrentCpuUsage()
    {
        var searcher = new ManagementObjectSearcher("select * from Win32_Processor");
        var queryCollection = searcher.Get();

        return queryCollection.Cast<ManagementObject>().Average(mo => Convert.ToDouble(mo["LoadPercentage"]));
    }

    public double GetTotalStorageInGB()
    {
        double totalStorage = 0;

        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady)
            {
                totalStorage += drive.TotalSize;
            }
        }

        return ConvertBytesToGigabytes(totalStorage);
    }

    public double GetUsedStorageInGB()
    {
        double usedStorage = 0;

        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady)
            {
                usedStorage += (drive.TotalSize - drive.AvailableFreeSpace);
            }
        }

        return ConvertBytesToGigabytes(usedStorage);
    }

    public float GetCpuTemperature()
    {
        OpenHardwareMonitor.Hardware.Computer computer = new OpenHardwareMonitor.Hardware.Computer()
        {
            CPUEnabled = true
        };
        computer.Open();

        float temperature = float.NaN;

        foreach (IHardware hardware in computer.Hardware)
        {
            if (hardware.HardwareType == HardwareType.CPU)
            {
                hardware.Update();
                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == SensorType.Temperature)
                    {
                        temperature = sensor.Value.GetValueOrDefault();
                    }
                }
            }
        }

        computer.Close();
        return temperature;
    }

    public int GetTotalCpuCores() => Environment.ProcessorCount;

    private static double ConvertBytesToGigabytes(ulong bytes) => bytes / 1024.0 / 1024.0 / 1024.0;

    private static double ConvertBytesToGigabytes(double bytes) => bytes / 1024 / 1024 / 1024;
}