using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LibVLCSharp.Shared;
using SubtitleEditor.ViewModels;
using SubtitleEditor.Views;

namespace SubtitleEditor
{
    public class App : Application
    {
        public override void Initialize()
        {
            Core.Initialize();
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
                desktop.Exit += OnExit;

            }

            
            base.OnFrameworkInitializationCompleted();
        }
        void OnExit(object sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var vm = (MainWindowViewModel)desktop.MainWindow?.DataContext;
                if (vm != null)
                    vm.Dispose();
            }
        }

    }
}