using System.Windows;
using System.Windows.Input;
using ItemEditor.Commands;
using ItemEditor.Navigation;
using Rollback.Common.ORM;

namespace ItemEditor.ViewModels
{
    internal sealed class ChooseEditingViewModel : ViewModelBase<ChooseEditingWindow>
    {
        private readonly DatabaseAccessor _accessor;

        public ICommand CloseCommand =>
            new RelayCommand(_ => View.Close());

        private RelayCommand? _minCommand;
        public ICommand MinCommand =>
            _minCommand ??= new RelayCommand(_ =>
                View.WindowState = WindowState.Minimized);

        private RelayCommand? _editItemCommand;
        public ICommand EditItemCommand =>
            _editItemCommand ??= new RelayCommand(_ => NavigationSystem.ChangeMainWindow(new EditItemViewModel(_accessor).View));

        private RelayCommand? _editPetCommand;
        public ICommand EditPetCommand =>
            _editPetCommand ??= new RelayCommand(_ => NavigationSystem.ChangeMainWindow(new EditPetItemViewModel(_accessor).View));

        private RelayCommand? _editMountsCommand;
        public ICommand EditMountsCommand =>
            _editMountsCommand ??= new RelayCommand(_ => NavigationSystem.ChangeMainWindow(new EditMountEffectsViewModel(_accessor).View));

        public ChooseEditingViewModel(DatabaseAccessor accessor)
            : base() =>
            _accessor = accessor;
    }
}
