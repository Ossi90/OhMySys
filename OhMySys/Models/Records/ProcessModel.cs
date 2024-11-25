using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;

namespace OhMySys.Models.Records;

public class ProcessModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string MainWindowTitle { get; init; } = string.Empty;
    public string InnerName { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public long Memory { get; init; }
    public ProcessStatus Status { get; init; }
    public SystemImpact Impact { get; init; }
    public double CpuUsage { get; init; }
    public BitmapSource? Icon { get; init; }

    // Bilad her
    public SolidColorBrush ImpactColor => GetImpactolor();
    public SolidColorBrush StatusColor => GetStatusColor();

    private SolidColorBrush GetStatusColor()
    {
        return Application.Current.Dispatcher.Invoke(() =>
        {
            return Status switch
            {
                ProcessStatus.NotResponding => Brushes.LightGray,
                _ => Brushes.White
            };
        });
    }

    private SolidColorBrush GetImpactolor()
    {
        return Application.Current.Dispatcher.Invoke(() =>
         {
             return Impact switch
             {
                 SystemImpact.Low => Brushes.Green,
                 SystemImpact.High => Brushes.Red,
                 SystemImpact.Medium => Brushes.Orange,
                 _ => Brushes.Black
             };
         });
    }
}

public enum ProcessStatus
{
    Running,
    Stopped,
    NotResponding
}

public enum SystemImpact
{
    Low,
    Medium,
    High
}