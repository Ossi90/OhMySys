using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace OhMySys;

public partial class MainWindow : Window
{
    private TaskbarIcon _notifyIcon;
    private bool IsVisible { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        CreateTrayIcon();

        this.ShowInTaskbar = false;

        this.Hide();

        IsVisible = false;

        this.Closing += MainWindow_Closing;
    }

    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // Cancel the event and hide the window
        e.Cancel = true;
        this.Hide();
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
        if (!IsVisible)
        {
            ShowWindow();
            IsVisible = true;
        }
        else
        {
            HideWindow();
            IsVisible = false;
        }
    }

    private void ShowWindow()
    {
        // Set the initial position of the window to the bottom of the screen
        this.Left = SystemParameters.WorkArea.Width - this.Width;
        this.Top = SystemParameters.WorkArea.Height;

        this.Show();
        this.WindowState = WindowState.Normal;
        this.Activate();

        // Create a DoubleAnimation to animate the Top property
        var animation = new DoubleAnimation
        {
            To = SystemParameters.WorkArea.Height - this.Height, // The final value of the Top property
            Duration = TimeSpan.FromSeconds(0.5) // The duration of the animation
        };

        // Start the animation
        this.BeginAnimation(Window.TopProperty, animation);
    }

    private void HideWindow()
    {
        this.Hide();
    }

    private void Exit_Click(object sender, EventArgs e)
    {
        _notifyIcon.Dispose();
        Application.Current.Shutdown();
    }

    private void Show_Click(object sender, RoutedEventArgs e)
    {
        ShowWindow();
    }

    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            this.Hide();
        }
        else
        {
            this.Show();
        }
        base.OnStateChanged(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        this.Hide();
    }
}