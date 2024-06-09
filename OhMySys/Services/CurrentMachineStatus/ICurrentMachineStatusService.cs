namespace OhMySys.Services.CurrentMachineStatus;

public interface ICurrentMachineStatusService
{
    double GetUsedMemoryInGB();

    double GetTotalMemoryInGB();

    double GetCurrentCpuUsage();

    int GetTotalCpuCores();

    public double GetTotalStorageInGB();

    public double GetUsedStorageInGB();

    float GetCpuTemperature();
}