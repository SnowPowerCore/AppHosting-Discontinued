using AsyncAwaitBestPractices.MVVM;
using System;
using System.Threading;
using System.Windows.Input;

namespace AppHosting.Xamarin.Forms.Utils.Commands
{
#nullable enable
    /// <summary>
    /// A command whose sole purpose is to relay its functionality 
    /// to other objects by invoking delegates. 
    /// The default return value for the CanExecute method is 'true'.
    /// <see cref="CanExecute"/> is expected to return a different value.
    /// </summary>
    public abstract class BaseRelayCommand<T> : BaseCommand<T>, ICommand
    {
        private readonly Action<T> _execute;

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public BaseRelayCommand(Action<T> execute, Func<bool> canExecute) : base(_ => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
        }

        public bool CanExecute(object parameter) => parameter switch
        {
            object validParameter => base.CanExecute((T)validParameter),
            null when IsNullable<T>() => base.CanExecute((T)parameter!),
            null => throw new InvalidCommandParameterException(typeof(object)),
        };

        /// <summary>
        /// Executes the <see cref="RelayCommand"/> on the current command target.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public void Execute(object? parameter)
        {
            Interlocked.Exchange(ref _isExecuting, 1);
            RaiseCanExecuteChanged();

            try
            {
                _execute((T)parameter!);
            }
            finally
            {
                Interlocked.Exchange(ref _isExecuting, 0);
                RaiseCanExecuteChanged();
            }
        }
    }

    public class RelayCommand : BaseRelayCommand<object>
    {
        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute)
            : base(obj => execute?.Invoke(), null!) { }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
            : base(obj => execute?.Invoke(), canExecute) { }
    }

    public class RelayCommand<T> : BaseRelayCommand<T>
    {
        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<T> execute)
            : base(execute, null!) { }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Func<bool> canExecute)
            : base(execute, canExecute) { }
    }
#nullable disable
}