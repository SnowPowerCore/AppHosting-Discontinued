using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppHosting.Xamarin.Forms.Utils.Commands
{
#nullable enable
    /// <summary>
    /// Abstract Base Class used by AsyncValueCommand
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BaseAsyncRelayCommand<TExecute, TCanExecute> : BaseCommand<TCanExecute>, ICommand
    {
        private readonly Func<TExecute, Task> _execute;
        private readonly Action<Exception>? _onException;
        private readonly bool _continueOnCapturedContext;

        /// <summary>
        /// Initializes a new instance of BaseAsyncCommand
        /// </summary>
        /// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is default, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        protected private BaseAsyncRelayCommand(Func<TExecute, Task>? execute,
            Func<TCanExecute, bool>? canExecute,
            Action<Exception>? onException,
            bool continueOnCapturedContext) : base(canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), $"{ nameof(execute) } cannot be default");
            _onException = onException;
            _continueOnCapturedContext = continueOnCapturedContext;
        }

        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The executed Task</returns>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to default.</param>
        private protected Task ExecuteAsync(TExecute parameter) => _execute(parameter);

        public bool CanExecute(object parameter) => parameter switch
        {
            TCanExecute validParameter => base.CanExecute(validParameter),
            null when IsNullable<TCanExecute>() => base.CanExecute((TCanExecute)parameter!),
            null => throw new InvalidCommandParameterException(typeof(TCanExecute)),
            _ => throw new InvalidCommandParameterException(typeof(TCanExecute), parameter.GetType()),
        };

        public void Execute(object? parameter)
        {
            Interlocked.Exchange(ref _isExecuting, 1);
            RaiseCanExecuteChanged();
            switch (parameter)
            {
                case TExecute validParameter:
                    ExecuteAsync(validParameter)
                        .ContinueWith(t =>
                        {
                            Interlocked.Exchange(ref _isExecuting, 0);
                            RaiseCanExecuteChanged();
                        }, TaskContinuationOptions.NotOnFaulted)
                        .SafeFireAndForget(e =>
                        {
                            Interlocked.Exchange(ref _isExecuting, 0);
                            RaiseCanExecuteChanged();
                            _onException?.Invoke(e);
                        }, _continueOnCapturedContext);
                    break;

                case null when IsNullable<TExecute>():
                    ExecuteAsync((TExecute)parameter!)
                        .ContinueWith(t =>
                        {
                            Interlocked.Exchange(ref _isExecuting, 0);
                            RaiseCanExecuteChanged();
                        }, TaskContinuationOptions.NotOnFaulted)
                        .SafeFireAndForget(e =>
                        {
                            Interlocked.Exchange(ref _isExecuting, 0);
                            RaiseCanExecuteChanged();
                            _onException?.Invoke(e);
                        }, _continueOnCapturedContext);
                    break;

                case null:
                    throw new InvalidCommandParameterException(typeof(TExecute));

                default:
                    throw new InvalidCommandParameterException(typeof(TExecute), parameter.GetType());
            }
        }
    }

    /// <summary>
    /// An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    public class AsyncRelayCommand<TExecute, TCanExecute> : BaseAsyncRelayCommand<TExecute, TCanExecute>, IAsyncCommand<TExecute, TCanExecute>
    {
        /// <summary>
        /// Initializes a new instance AsyncCommand
        /// </summary>
        /// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is default, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public AsyncRelayCommand(Func<TExecute, Task> execute,
                            Func<TCanExecute, bool>? canExecute = default,
                            Action<Exception>? onException = default,
                            bool continueOnCapturedContext = false)
            : base(execute, canExecute, onException, continueOnCapturedContext)
        {

        }

        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The executed Task</returns>
        public new Task ExecuteAsync(TExecute parameter) => base.ExecuteAsync(parameter);
    }

    /// <summary>
    /// An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    public class AsyncRelayCommand<T> : BaseAsyncRelayCommand<T, object?>, IAsyncCommand<T>
    {
        /// <summary>
        /// Initializes a new instance AsyncCommand
        /// </summary>
        /// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is default, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public AsyncRelayCommand(Func<T, Task> execute,
                            Func<object?, bool>? canExecute = default,
                            Action<Exception>? onException = default,
                            bool continueOnCapturedContext = false)
            : base(execute, canExecute, onException, continueOnCapturedContext)
        {

        }

        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The executed Task</returns>
        public new Task ExecuteAsync(T parameter) => base.ExecuteAsync(parameter);
    }

    /// <summary>
    /// An implementation of IAsyncCommand. Allows Commands to safely be used asynchronously with Task.
    /// </summary>
    public class AsyncRelayCommand : BaseAsyncRelayCommand<object?, object?>, IAsyncCommand
    {
        /// <summary>
        /// Initializes a new instance of AsyncCommand
        /// </summary>
        /// <param name="execute">The Function executed when Execute or ExecuteAsync is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
        /// <param name="canExecute">The Function that verifies whether or not AsyncCommand should execute.</param>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is default, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c> continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c> continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public AsyncRelayCommand(Func<Task> execute,
                            Func<object?, bool>? canExecute = default,
                            Action<Exception>? onException = default,
                            bool continueOnCapturedContext = false)
            : base(ConvertExecute(execute), canExecute, onException, continueOnCapturedContext)
        {
        }

        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The executed Task</returns>
        public Task ExecuteAsync() => ExecuteAsync(default);

        private static Func<object?, Task>? ConvertExecute(Func<Task> execute) =>
            execute is default(Func<Task>) ? default : (_ => execute());
    }
#nullable disable
}