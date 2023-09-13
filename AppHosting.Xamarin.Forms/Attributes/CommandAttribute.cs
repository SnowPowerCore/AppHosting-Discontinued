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

        public bool NativeAnimation { get; }

        public int HoveredAnimationDuration { get; }

        public int NormalAnimationDuration { get; }

        public CommandAttribute(
            string commandDelegateName,
            string commandName = "Command",
            string controlName = "",
            string commandObjectName = "",
            string commandCanExecuteDelegateName = "",
            bool nativeAnimation = false,
            int hoveredAnimationDuration = 500,
            int normalAnimationDuration = 100)
        {
            ControlName = controlName;
            CommandDelegateName = commandDelegateName;
            CommandName = commandName;
            CommandObjectName = commandObjectName;
            CommandCanExecuteDelegateName = commandCanExecuteDelegateName;
            NativeAnimation = nativeAnimation;
            HoveredAnimationDuration = hoveredAnimationDuration;
            NormalAnimationDuration = normalAnimationDuration;
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
            string commandCanExecuteDelegateName = "",
            bool nativeAnimation = false,
            int hoveredAnimationDuration = 500,
            int normalAnimationDuration = 100)
            : base(commandDelegateName,
                   controlName: controlName,
                   commandObjectName: commandObjectName,
                   commandCanExecuteDelegateName: commandCanExecuteDelegateName,
                   nativeAnimation: nativeAnimation,
                   hoveredAnimationDuration: hoveredAnimationDuration,
                   normalAnimationDuration: normalAnimationDuration)
        { }
    }

    /// <summary>
    /// Please, use it on a page class. You can consume this multiple times.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AttachedLongPressCommandAttribute : CommandAttribute
    {
        public int LongPressDuration { get; }

        public int PressedAnimationDuration { get; }

        public AttachedLongPressCommandAttribute(
            string commandDelegateName,
            int longPressDuration = 500,
            string controlName = "",
            string commandObjectName = "",
            string commandCanExecuteDelegateName = "",
            bool nativeAnimation = false,
            int hoveredAnimationDuration = 500,
            int normalAnimationDuration = 100,
            int pressedAnimationDuration = 100)
            : base(commandDelegateName,
                   controlName: controlName,
                   commandObjectName: commandObjectName,
                   commandCanExecuteDelegateName: commandCanExecuteDelegateName,
                   nativeAnimation: nativeAnimation,
                   hoveredAnimationDuration: hoveredAnimationDuration,
                   normalAnimationDuration: normalAnimationDuration)
        {
            LongPressDuration = longPressDuration;
            PressedAnimationDuration = pressedAnimationDuration;
        }
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
            string onException = "",
            bool nativeAnimation = false,
            int hoveredAnimationDuration = 500,
            int normalAnimationDuration = 100)
            : base(commandTaskName, commandName, controlName,
                   commandObjectName, commandCanExecuteDelegateName,
                   nativeAnimation: nativeAnimation,
                   hoveredAnimationDuration: hoveredAnimationDuration,
                   normalAnimationDuration: normalAnimationDuration)
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
            string onException = "",
            bool nativeAnimation = false,
            int hoveredAnimationDuration = 500,
            int normalAnimationDuration = 100)
            : base(commandTaskName,
                   controlName: controlName,
                   commandObjectName: commandObjectName,
                   commandCanExecuteDelegateName: commandCanExecuteDelegateName,
                   continueOnCapturedContext: continueOnCapturedContext,
                   onException: onException,
                   nativeAnimation: nativeAnimation,
                   hoveredAnimationDuration: hoveredAnimationDuration,
                   normalAnimationDuration: normalAnimationDuration)
        { }
    }

    /// <summary>
    /// Please, use it on a page class. You can consume this multiple times.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AttachedAsyncLongPressCommandAttribute : AsyncCommandAttribute
    {
        public int LongPressDuration { get; }

        public int PressedAnimationDuration { get; }

        public AttachedAsyncLongPressCommandAttribute(
            string commandTaskName,
            int longPressDuration = 500,
            string controlName = "",
            string commandObjectName = "",
            string commandCanExecuteDelegateName = "",
            bool continueOnCapturedContext = false,
            string onException = "",
            bool nativeAnimation = false,
            int hoveredAnimationDuration = 500,
            int normalAnimationDuration = 100,
            int pressedAnimationDuration = 100)
            : base(commandTaskName,
                   controlName: controlName,
                   commandObjectName: commandObjectName,
                   commandCanExecuteDelegateName: commandCanExecuteDelegateName,
                   continueOnCapturedContext: continueOnCapturedContext,
                   onException: onException,
                   nativeAnimation: nativeAnimation,
                   hoveredAnimationDuration: hoveredAnimationDuration,
                   normalAnimationDuration: normalAnimationDuration)
        {
            LongPressDuration = longPressDuration;
            PressedAnimationDuration = pressedAnimationDuration;
        }
    }
}