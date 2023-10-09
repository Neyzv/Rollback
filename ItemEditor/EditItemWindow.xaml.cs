using System.Windows;

namespace ItemEditor
{
    /// <summary>
    /// Logique d'interaction pour EditItemWindow.xaml
    /// </summary>
    public partial class EditItemWindow : Window
    {
        public EditItemWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            DragMove();
    }
}
