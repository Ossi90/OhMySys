using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;

namespace OhMySys.Models.Records;

public class ProcessModel : INotifyPropertyChanged
{
    private ProcessStatus _status;
    private SystemImpact _impact;

    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string MainWindowTitle { get; init; } = string.Empty;
    public string InnerName { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public long Memory { get; init; }
    public double CpuUsage { get; init; }
    public BitmapSource? Icon { get; init; }

    public ProcessStatus Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusColor)); // Notify for dependent property
            }
        }
    }

    public SystemImpact Impact
    {
        get => _impact;
        set
        {
            if (_impact != value)
            {
                _impact = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ImpactColor)); // Notify for dependent property
            }
        }
    }

    public SolidColorBrush ImpactColor => GetImpactColor();
    public SolidColorBrush StatusColor => GetStatusColor();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private SolidColorBrush GetImpactColor()
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