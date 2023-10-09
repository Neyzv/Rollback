using System.Windows;

namespace ItemEditor
{
    public partial class ChooseEditingWindow : Window
    {
        public ChooseEditingWindow() =>
            InitializeComponent();

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            DragMove();
    }
}
