using System.Windows;

namespace ItemEditor.Navigation
{
    internal static class NavigationSystem
    {
        public static void ChangeMainWindow(Window window)
        {
            var oldWindow = Application.Current.MainWindow;
            (Application.Current.MainWindow = window).Show();
            oldWindow.Close();
        }
    }
}
