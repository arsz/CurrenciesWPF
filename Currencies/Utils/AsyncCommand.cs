using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Utils.Interfaces;

namespace WpfApp.Utils
{
    public sealed class AsyncCommand : IAsyncCommand
    {
        private readonly Func<Task> execute;
        private readonly Predicate<object> canExecute;
        private readonly Action<Exception> onException;
        private readonly bool continueOnCapturedContext;

        public AsyncCommand(Func<Task> execute,
                            Predicate<object> canExecute = null,
                            Action<Exception> onException = null,
                            bool continueOnCapturedContext = true)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} cannot be null");
            this.canExecute = canExecute;
            this.onException = onException;
            this.continueOnCapturedContext = continueOnCapturedContext;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public Task ExecuteAsync() => execute();

        void ICommand.Execute(object parameter) => execute().FireAndForget(continueOnCapturedContext, onException);
    }
}