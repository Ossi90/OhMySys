using CommunityToolkit.Mvvm.ComponentModel;
using OhMySys.Common.Helpers;
using OhMySys.Services.CurrentMachineStatus;

namespace OhMySys.Models;

public partial class MainWindowViewModel : ObservableRecipient
{
    private readonly ICurrentMachineStatusService _currentMachineStatusService;
    private readonly Timer MachineCheckTimer;

    public MainWindowViewModel(ICurrentMachineStatusService currentMachineStatusService)
    {
        _currentMachineStatusService = currentMachineStatusService;

        // Set up a timer to update the CPU usage every second
        MachineCheckTimer = new Timer(UpdateMachineStatus, null, 0, 1000);
    }

    [ObservableProperty]
    private double usedMemoryInGB;

    [ObservableProperty]
    private double totalMemoryInGB;

    [ObservableProperty]
    private float currentCpuUsage;

    [ObservableProperty]
    private int totalCpuCores;

    [ObservableProperty]
    private double totalStorageInGB;

    [ObservableProperty]
    private double usedStorageInGB;

    [ObservableProperty]
    private float cpuTemperature;

    [ObservableProperty]
    private string cpuTemperatureText;

    [ObservableProperty]
    private string storageUsageText;

    [ObservableProperty]
    private string cpuUsageText;

    [ObservableProperty]
    private string memoryUsageText;

    public void UpdateMachineStatus(object state)
    {
        UsedMemoryInGB = Math.Round(_currentMachineStatusService.GetUsedMemoryInGB(), 1);
        TotalMemoryInGB = Math.Round(_currentMachineStatusService.GetTotalMemoryInGB(), 1);
        CurrentCpuUsage = (float)Math.Round(_currentMachineStatusService.GetCurrentCpuUsage(), 1); // Initial CPU usage
        TotalCpuCores = _currentMachineStatusService.GetTotalCpuCores();
        TotalStorageInGB = Math.Round(_currentMachineStatusService.GetTotalStorageInGB(), 1);
        UsedStorageInGB = Math.Round(_currentMachineStatusService.GetUsedStorageInGB(), 1);
        //CpuTemperature = (float)Math.Round(_currentMachineStatusService.GetCpuTemperature(), 1);

        CpuTemperatureText = $"{CpuTemperature:0.0}°";
        StorageUsageText = StringSizeConverter.ConvertBytesToReadableString([UsedStorageInGB, TotalStorageInGB]);
        CpuUsageText = $"{CurrentCpuUsage:0.0}%";
        MemoryUsageText = StringSizeConverter.ConvertBytesToReadableString([UsedMemoryInGB, TotalMemoryInGB]);
    }
}