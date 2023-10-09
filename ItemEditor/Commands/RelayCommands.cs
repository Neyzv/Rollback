using System;
using System.Windows.Input;

namespace ItemEditor.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object?> action, Predicate<object?>? p = default)
        {
            _execute = action;
            _canExecute = p;
        }

        public bool CanExecute(object? parameter) =>
            _canExecute is null || _canExecute(parameter);

        public void Execute(object? parameter) =>
            _execute(parameter);
    }

    internal class RelayCommand<TParam> : ICommand
    {
        private readonly Action<TParam> _execute;
        private readonly Predicate<TParam>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<TParam> action, Predicate<TParam>? p = default)
        {
            _execute = action;
            _canExecute = p;
        }

        public bool CanExecute(object? parameter) =>
            _canExecute is null || _canExecute((TParam)parameter!);

        public void Execute(object? parameter) =>
            _execute((TParam)parameter!);
    }
}
