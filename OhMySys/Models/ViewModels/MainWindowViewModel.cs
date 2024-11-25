using CommunityToolkit.Mvvm.ComponentModel;
using OhMySys.Common.Helpers;
using OhMySys.Models.Records;
using OhMySys.Services.CurrentMachineStatus;
using OhMySys.Services.ProcessService;
using System.Collections.ObjectModel;
using System.Linq;

namespace OhMySys.Models;

public partial class MainWindowViewModel : ObservableRecipient, IDisposable
{
    private readonly ICurrentMachineStatusService _currentMachineStatusService;
    private readonly IProcessService _processService;
    private Timer MachineCheckTimer;

    public MainWindowViewModel(ICurrentMachineStatusService currentMachineStatusService, IProcessService processService)
    {
        _currentMachineStatusService = currentMachineStatusService;
        _processService = processService;
        MachineCheckTimer = new Timer(UpdateUI, null, 0, 1000);
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

    [ObservableProperty]
    private ObservableCollection<ProcessModel> _activeApps = new();

    [ObservableProperty]
    private string _activeAppsCount = "Apps ({0})";

    public void UpdateUI(object state)
    {
        try
        {
            SetMachineStatus();
            SetText();
            UpdateUserProcesses();
        }
        catch (Exception ex)
        {
        }
    }

    private void SetText()
    {
        CpuTemperatureText = $"{CpuTemperature:0.0}°";
        StorageUsageText = StringSizeConverter.ConvertBytesToReadableString([UsedStorageInGB, TotalStorageInGB]);
        CpuUsageText = $"{CurrentCpuUsage:0.0}%";
        MemoryUsageText = StringSizeConverter.ConvertBytesToReadableString([UsedMemoryInGB, TotalMemoryInGB]);
    }

    private void SetMachineStatus()
    {
        UsedMemoryInGB = Math.Round(_currentMachineStatusService.GetUsedMemoryInGB(), 1);
        TotalMemoryInGB = Math.Round(_currentMachineStatusService.GetTotalMemoryInGB(), 1);
        CurrentCpuUsage = (float)Math.Round(_currentMachineStatusService.GetCurrentCpuUsage(), 1);
        TotalCpuCores = _currentMachineStatusService.GetTotalCpuCores();
        TotalStorageInGB = Math.Round(_currentMachineStatusService.GetTotalStorageInGB(), 1);
        UsedStorageInGB = Math.Round(_currentMachineStatusService.GetUsedStorageInGB(), 1);
        // CpuTemperature = (float)Math.Round(_currentMachineStatusService.GetCpuTemperature(), 1);
    }

    private void UpdateUserProcesses()
    {
        var processes = _processService.GetActiveWindowProcesses();
        ActiveApps = new();
        OnPropertyChanged(nameof(ActiveApps));

        foreach (var activeApp in processes)
        {
            ActiveApps?.Add(activeApp);
        }

        ActiveAppsCount = string.Format(ActiveAppsCount, ActiveApps.Count);
    }

    public void Dispose()
    {
        //MachineCheckTimer.Dispose();
    }
}