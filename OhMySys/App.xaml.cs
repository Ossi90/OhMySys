using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OhMySys.Models;
using OhMySys.Services.CurrentMachineStatus;
using System.Windows;

namespace OhMySys
{
    public partial class App : Application
    {
        public IHost Host
        {
            get;
        }

        public App()
        {
            Host = CreateHostBuilder().Build();
        }

        public static T GetService<T>() where T : class
        {
            if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
            }

            return service;
        }

        public IHostBuilder CreateHostBuilder()
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().
                UseContentRoot(AppContext.BaseDirectory).
                ConfigureServices((context, services) =>
                {
                    services.AddScoped<ICurrentMachineStatusService, CurrentMachineStatusService>();

                    services.AddTransient<MainWindow>();
                    services.AddTransient<MainWindowViewModel>();
                });
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await Host.StartAsync();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (Host)
            {
                await Host.StopAsync(TimeSpan.FromSeconds(5));
            }
            base.OnExit(e);
        }
    }
}