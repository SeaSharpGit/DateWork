using DateWork.Controls;
using System.Windows;
using System.Windows.Threading;

namespace DateWork
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherUnhandledException += App_DispatcherUnhandledException;

        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageWindow.Show(e.Exception.Message, "错误", false);
            e.Handled = true;
        }
    }
}
