using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BleemSync.ViewModels;
using BleemSync.Views;
using BleemSync.Data;
using BleemSync.Services;

namespace BleemSync
{
    public class App : Application
    {

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            using (var platformService = new PlatformService())
            {
                platformService.Initialize();
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
