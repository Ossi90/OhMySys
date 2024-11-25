using Hardcodet.Wpf.TaskbarNotification;
using OhMySys.Common.Factories;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace OhMySys.Common.Handlers
{
    public class TrayIconHandler : IDisposable

    {
        private readonly MainWindow _window;
        private readonly TaskbarIcon _notifyIcon;
        private DispatcherTimer HideTimer { get; set; }
        private bool IsWindowVisible { get; set; }

        public TrayIconHandler(MainWindow window)
        {
            _window = window;
            IsWindowVisible = false;
            _notifyIcon = TaskBarIconFactory.Create();
            HideTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            HideTimer.Tick += HideTimer_Tick;

            RegisterEventHandlers();
        }

        private void RegisterEventHandlers()
        {
            _window.MouseLeave += Window_MouseLeave;
            _window.MouseEnter += Window_MouseEnter;
            _notifyIcon.TrayLeftMouseUp += NotifyIcon_Click;
            _window.Closing += Window_Closing;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            HideTimer.Start();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            HideTimer.Stop();
        }

        private void HideTimer_Tick(object sender, EventArgs e)
        {
            HideWindow();
            HideTimer.Stop();
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
            _window.Left = SystemParameters.WorkArea.Width - _window.Width;
            _window.Top = SystemParameters.WorkArea.Height + 1;

            _window.Show();
            _window.WindowState = WindowState.Normal;
            _window.Activate();
            _window.Focus();

            var animation = new DoubleAnimation
            {
                To = SystemParameters.WorkArea.Height - _window.Height + 10,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            _window.BeginAnimation(Window.TopProperty, animation);
        }

        private void HideWindow()
        {
            var animation = new DoubleAnimation
            {
                To = SystemParameters.WorkArea.Height,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            animation.Completed += (s, e) =>
            {
                _window.Hide();
            };
            _window.BeginAnimation(Window.TopProperty, animation);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _window.Hide();
        }

        public void Dispose()
        {
            _notifyIcon.TrayLeftMouseUp -= NotifyIcon_Click;
            _notifyIcon.Dispose();

            _window.MouseLeave -= Window_MouseLeave;
            _window.MouseEnter -= Window_MouseEnter;
            _window.Closing -= Window_Closing;

            HideTimer.Tick -= HideTimer_Tick;
            HideTimer.Stop();
        }
    }
}