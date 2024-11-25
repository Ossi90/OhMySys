using OhMySys.Models.Records;

namespace OhMySys.Services.ProcessService;

public interface IProcessService
{
    List<ProcessModel> GetActiveWindowProcesses();
}