using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace OhMySys.Common.Factories
{
    public static class TaskBarIconFactory
    {
        public static TaskbarIcon Create(string? iconUrl = null, Visibility? visibility = null, ContextMenu? contextMenu = null)
        {
            return new TaskbarIcon
            {
                Icon = new Icon(iconUrl ?? "./Assets/eagle.ico", 40, 40),
                Visibility = visibility ?? Visibility.Visible,
                ContextMenu = contextMenu ?? (ContextMenu)Application.Current.Resources["NotifierContextMenu"]
            };
        }
    }
}