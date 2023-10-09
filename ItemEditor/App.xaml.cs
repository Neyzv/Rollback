using System.Windows;
using ItemEditor.ViewModels;

namespace ItemEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            (MainWindow = new MainViewModel().View).Show();
            base.OnStartup(e);
        }
    }
}
