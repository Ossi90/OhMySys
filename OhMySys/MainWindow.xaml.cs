using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace OhMySys;

public partial class MainWindow : Window
{
    private TaskbarIcon _notifyIcon;
    private bool IsWindowVisible { get; set; }
    private readonly DispatcherTimer _hideTimer;

    public MainWindow()
    {
        InitializeComponent();
        CreateTrayIcon();

        ShowInTaskbar = false;

        Hide();

        IsWindowVisible = false;

        Closing += MainWindow_Closing;
        MouseLeave += MainWindow_MouseLeave;
        MouseEnter += MainWindow_MouseEnter;

        // Initialize the timer
        _hideTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(2)
        };
        _hideTimer.Tick += HideTimer_Tick;
    }

    private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
    {
        _hideTimer.Start();
    }

    private void MainWindow_MouseEnter(object sender, MouseEventArgs e)
    {
        _hideTimer.Stop();
    }

    private void HideTimer_Tick(object sender, EventArgs e)
    {
        HideWindow();
        _hideTimer.Stop();
    }

    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }

    private void CreateTrayIcon()
    {
        _notifyIcon = new TaskbarIcon
        {
            Icon = new System.Drawing.Icon("./Assets/OhMySys.ico"),
            Visibility = Visibility.Visible
        };
        _notifyIcon.TrayLeftMouseUp += NotifyIcon_Click; // Handle single click

        var contextMenu = (ContextMenu)Resources["NotifierContextMenu"];
        _notifyIcon.ContextMenu = contextMenu;
    }

    private void NotifyIcon_Click(object sender, EventArgs e)
    {
        if (!IsWindowVisible)
        {
            ShowWindow();
            IsWindowVisible = true;
        }
        else
        {
            HideWindow();
            IsWindowVisible = false;
        }
    }

    private void ShowWindow()
    {
        Left = SystemParameters.WorkArea.Width - Width;
        Top = SystemParameters.WorkArea.Height + 1;

        Show();
        WindowState = WindowState.Normal;
        Activate();
        Focus(); // Add this line

        var animation = new DoubleAnimation
        {
            To = SystemParameters.WorkArea.Height - Height + 10,
            Duration = TimeSpan.FromSeconds(0.5)
        };

        // Start the animation
        BeginAnimation(Window.TopProperty, animation);
    }
    private void HideWindow()
    {
        var animation = new DoubleAnimation
        {
            To = SystemParameters.WorkArea.Height,
            Duration = TimeSpan.FromSeconds(0.5)
        };

        animation.Completed += (s, e) => Hide();

        BeginAnimation(Window.TopProperty, animation);
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        _notifyIcon.Dispose();
        System.Windows.Application.Current.Shutdown();
    }

    private void Show_Click(object sender, RoutedEventArgs e)
    {
        ShowWindow();
    }

    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            Hide();
        }
        else
        {
            Show();
        }
        base.OnStateChanged(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        Hide();
    }
}