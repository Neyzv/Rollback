using System.Windows.Input;
using ItemEditor.Commands;

namespace ItemEditor.ViewModels
{
    internal sealed class PopUpViewModel : ViewModelBase<PopUpWindow>
    {
        public ICommand CloseCommand =>
            new RelayCommand(_ => View.Close());

        public PopUpViewModel(string content)
            : base() =>
            View.Message.Text = content;
    }
}
