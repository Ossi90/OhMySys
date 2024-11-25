using OhMySys.Common.Handlers;
using OhMySys.Models;
using System.Windows;

namespace OhMySys;

public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; private set; }

    public MainWindow()
    {
        //TODO make sure time is disposed everytime the application goes down
        ViewModel = App.GetService<MainWindowViewModel>();

        InitializeComponent();

        DataContext = ViewModel;
        InitializeWindowProperties();
        _ = new TrayIconHandler(this);
    }

    private void InitializeWindowProperties()
    {
        ShowInTaskbar = false;
        Left = SystemParameters.WorkArea.Width - Width;
        Top = SystemParameters.WorkArea.Height - Height;
        Hide();
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Dispose();

        Application.Current.Shutdown();
    }

    protected override void OnClosed(EventArgs e)
    {
        Hide();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
    }

    private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
    }
}