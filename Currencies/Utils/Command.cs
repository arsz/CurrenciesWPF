using System;
using System.Windows.Input;

namespace WpfApp.Utils
{
    public class Command : ICommand
    {
        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        public Command(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => execute(parameter);
    }

    public class Command<T> : ICommand
    {
        private readonly Predicate<T> canExecute;
        private readonly Action<T> execute;

        public Command(Action<T> execute, Predicate<T> canExecute = null)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke((T)parameter) ?? true;

        public void Execute(object parameter) => execute((T)parameter);
    }
}