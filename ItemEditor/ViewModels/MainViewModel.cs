using System.Windows;
using System.Windows.Input;
using ItemEditor.Commands;
using ItemEditor.Navigation;
using Rollback.Common.ORM;

namespace ItemEditor.ViewModels
{
    internal sealed class MainViewModel : ViewModelBase<MainWindow>
    {
        public ICommand CloseCommand =>
            new RelayCommand(_ => View.Close());

        private RelayCommand? _minCommand;
        public ICommand MinCommand =>
            _minCommand ??= new RelayCommand(_ =>
                View.WindowState = WindowState.Minimized);

        private RelayCommand? _connectCommand;
        public ICommand ConnectCommand =>
            _connectCommand ??= new RelayCommand(_ => ConnectToDatabase());

        public MainViewModel(MainWindow view) : base(view) { }

        public MainViewModel() : base() { }

        private void ConnectToDatabase()
        {
            var db = new DatabaseAccessor(new()
            {
                Host = View.Host.Text,
                User = View.User.Text,
                Password = View.Password.Text,
                DatabaseName = View.DBName.Text,
            });

            if (db.TestConnection())
                NavigationSystem.ChangeMainWindow(new ChooseEditingViewModel(db).View);
            else
                new PopUpViewModel("Error while connecting to the database, please check that your mysql provider is on or your identification logs...")
                    .View.ShowDialog();
        }
    }
}
