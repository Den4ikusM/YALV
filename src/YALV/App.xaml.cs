using System.Threading;
using System.Windows;
using SimpleInjector;
using YALV.Common;

namespace YALV
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceProvider = ServiceProvider.Build(container =>
            {
                container.RegisterSingleton<ICommandLineArgs>(() => new CommandLineArgs(e.Args));
                container.RegisterSingleton<ISelectedCultureAccessor>(() => new InMemoryCultureAccessor());
                container.RegisterSingleton<MainWindow>();
            });
            var cultureAccessor = serviceProvider.GetService<ISelectedCultureAccessor>();
            Thread.CurrentThread.CurrentCulture = cultureAccessor.GetCulture();
            Thread.CurrentThread.CurrentUICulture = cultureAccessor.GetCulture();

            int? framerate = FrameRateHelper.DesiredFrameRate;
            BusyIndicatorBehavior.FRAMERATE = framerate;
            FrameRateHelper.SetTimelineDefaultFramerate(framerate);

            var window = serviceProvider.GetService<MainWindow>();
            window.Show();
        }
    }
}
