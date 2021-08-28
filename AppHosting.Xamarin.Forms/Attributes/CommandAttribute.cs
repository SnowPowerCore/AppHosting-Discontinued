using System;

namespace AppHosting.Xamarin.Forms.Attributes
{
    /// <summary>
    /// Please, use it on a page class. You can consume this multiple times.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommandAttribute : Attribute
    {
        public string ControlName { get; } = string.Empty;

        public string CommandDelegateName { get; } = string.Empty;

        public string CommandName { get; }

        public string CommandCanExecuteDelegateName { get; } = string.Empty;

        public string CommandObjectName { get; }

        public CommandAttribute(
            string commandDelegateName,
            string commandName = "Command",
            string controlName = "",
            string commandObjectName = "",
            string commandCanExecuteDelegateName = "")
        {
            ControlName = controlName;
            CommandDelegateName = commandDelegateName;
            CommandName = commandName;
            CommandObjectName = commandObjectName;
            CommandCanExecuteDelegateName = commandCanExecuteDelegateName;
        }
    }

    /// <summary>
    /// Please, use it on a page class. You can consume this multiple times.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AttachedCommandAttribute : CommandAttribute
    {
        public AttachedCommandAttribute(
            string commandDelegateName,
            string controlName = "",
            string commandObjectName = "",
            string commandCanExecuteDelegateName = "")
            : base(commandDelegateName,
                   controlName: controlName,
                   commandObjectName: commandObjectName,
                   commandCanExecuteDelegateName: commandCanExecuteDelegateName)
        { }
    }

    /// <summary>
    /// Please, use it on a page class. You can consume this multiple times.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AsyncCommandAttribute : CommandAttribute
    {
        public bool ContinueOnCapturedContext { get; }

        public string OnException { get; }

        public AsyncCommandAttribute(
            string commandTaskName,
            string commandName = "Command",
            string controlName = "",
            string commandObjectName = "",
            string commandCanExecuteDelegateName = "",
            bool continueOnCapturedContext = false,
            string onException = "")
            : base(commandTaskName, commandName, controlName,
                  commandObjectName, commandCanExecuteDelegateName)
        {
            ContinueOnCapturedContext = continueOnCapturedContext;
            OnException = onException;
        }
    }

    /// <summary>
    /// Please, use it on a page class. You can consume this multiple times.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AttachedAsyncCommandAttribute : AsyncCommandAttribute
    {
        public AttachedAsyncCommandAttribute(
            string commandTaskName,
            string controlName = "",
            string commandObjectName = "",
            string commandCanExecuteDelegateName = "",
            bool continueOnCapturedContext = false,
            string onException = "")
            : base(commandTaskName,
                   controlName: controlName,
                   commandObjectName: commandObjectName,
                   commandCanExecuteDelegateName: commandCanExecuteDelegateName,
                   continueOnCapturedContext: continueOnCapturedContext,
                   onException: onException)
        { }
    }
}