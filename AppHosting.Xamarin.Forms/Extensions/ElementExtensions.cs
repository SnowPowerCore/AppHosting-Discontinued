using AppHosting.Xamarin.Forms.Attributes;
using AppHosting.Xamarin.Forms.Utils.Commands;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamEffects;

namespace AppHosting.Xamarin.Forms.Extensions
{
    public static class ElementExtensions
    {
        public static Element AddAttachedCommands(
            this Element xfElement,
            AttachedCommandAttribute[] commandAttrs,
            object differentBindingContext = default)
        {
            foreach (var commandAttr in commandAttrs)
            {
                if (string.IsNullOrEmpty(commandAttr.CommandDelegateName))
                    continue;

                var desiredControl = xfElement.GetControlData(commandAttr.ControlName,
                    out _, out var bindingContextType);

                bindingContextType ??= differentBindingContext?.GetType();

                AssignAttachedCommandParameter(
                    desiredControl, xfElement.BindingContext ?? differentBindingContext,
                    commandAttr);

                AssignAttachedCommand(
                    desiredControl, xfElement.BindingContext ?? differentBindingContext,
                    bindingContextType,
                    commandAttr);
            }
            return xfElement;
        }

        public static Element AddAttachedAsyncCommands(
            this Element xfElement,
            AttachedAsyncCommandAttribute[] commandAttrs,
            object differentBindingContext = default)
        {
            foreach (var commandAttr in commandAttrs)
            {
                if (string.IsNullOrEmpty(commandAttr.CommandDelegateName))
                    continue;

                var desiredControl = xfElement.GetControlData(commandAttr.ControlName,
                    out _, out var bindingContextType);

                bindingContextType ??= differentBindingContext?.GetType();

                AssignAttachedCommandParameter(
                    desiredControl, xfElement.BindingContext ?? differentBindingContext,
                    commandAttr);

                AssignAttachedAsyncCommand(
                    desiredControl, xfElement.BindingContext ?? differentBindingContext,
                    bindingContextType,
                    commandAttr);
            }
            return xfElement;
        }

        public static Element AddCommands(
            this Element xfElement,
            CommandAttribute[] commandAttrs,
            object differentBindingContext = default)
        {
            foreach (var commandAttr in commandAttrs)
            {
                if (string.IsNullOrEmpty(commandAttr.CommandDelegateName))
                    continue;

                var desiredControl = xfElement.GetControlData(commandAttr.ControlName,
                    out var desiredControlType, out var bindingContextType);

                bindingContextType ??= differentBindingContext?.GetType();

                AssignCommandParameter(
                    desiredControl, desiredControlType, bindingContextType, commandAttr);

                AssignCommand(
                    desiredControl, xfElement.BindingContext ?? differentBindingContext,
                    desiredControlType, bindingContextType,
                    commandAttr);
            }
            return xfElement;
        }

        public static Element AddAsyncCommands(
            this Element xfElement,
            AsyncCommandAttribute[] commandAttrs,
            object differentBindingContext = default)
        {
            foreach (var commandAttr in commandAttrs)
            {
                if (string.IsNullOrEmpty(commandAttr.CommandDelegateName))
                    continue;

                var desiredControl = xfElement.GetControlData(commandAttr.ControlName,
                    out var desiredControlType, out var bindingContextType);

                bindingContextType ??= differentBindingContext?.GetType();

                AssignCommandParameter(
                    desiredControl, desiredControlType, bindingContextType, commandAttr);

                AssignAsyncCommand(
                    desiredControl, xfElement.BindingContext ?? differentBindingContext,
                    desiredControlType, bindingContextType,
                    commandAttr);
            }
            return xfElement;
        }

        private static void AssignAttachedCommandParameter(
            BindableObject control, object bindingContext, CommandAttribute commandAttr)
        {
            if (!string.IsNullOrEmpty(commandAttr.CommandObjectName))
            {
                var binding = new Binding(
                    commandAttr.CommandObjectName,
                    BindingMode.TwoWay,
                    source: bindingContext);

                control.SetBinding(Commands.TapParameterProperty, binding);
            }
        }

        private static void AssignCommandParameter(
            BindableObject control, Type controlType, Type bindingContextType, CommandAttribute commandAttr)
        {
            if (controlType.GetProperty("CommandParameter") is PropertyInfo)
            {
                var @object = bindingContextType
                    .GetProperty(commandAttr.CommandObjectName);

                if (@object != default)
                {
                    control.SetBinding(
                        (BindableProperty)controlType.GetField("CommandParameterProperty").GetValue(control),
                        commandAttr.CommandObjectName);
                }
            }
        }

        private static void AssignAttachedCommand(
            BindableObject control, object bindingContext,
            Type bindingContextType,
            CommandAttribute commandAttr)
        {
            var method = bindingContextType
                .GetMethod(commandAttr.CommandDelegateName);

            var canExecuteMethod = bindingContextType
                .GetMethod(commandAttr.CommandCanExecuteDelegateName);

            var command = new RelayCommand<object>(
                obj => _ = method.GetParameters().Count() <= 0
                    ? method.Invoke(bindingContext, new object[] { })
                    : method.Invoke(bindingContext, new object[] { obj }),
                () => canExecuteMethod != default
                    ? (bool)canExecuteMethod.Invoke(bindingContext, new object[0])
                    : true);

            Commands.SetTap(control, command);
        }

        private static void AssignAttachedAsyncCommand(
            BindableObject control, object bindingContext,
            Type bindingContextType,
            AsyncCommandAttribute commandAttr)
        {
            var method = bindingContextType
                .GetMethod(commandAttr.CommandDelegateName);

            var exceptionMethod = bindingContextType
                .GetMethod(commandAttr.OnException);

            var canExecuteMethod = bindingContextType
                .GetMethod(commandAttr.CommandCanExecuteDelegateName);

            var command = new AsyncRelayCommand<object>(
                obj => method.GetParameters().Count() <= 0
                    ? (Task)method.Invoke(bindingContext, new object[] { })
                    : (Task)method.Invoke(bindingContext, new object[] { obj }),
                canExecute: obj => canExecuteMethod != default
                    ? (bool)canExecuteMethod.Invoke(bindingContext, new object[0])
                    : true,
                onException: obj => exceptionMethod?.Invoke(bindingContext, new object[] { obj }),
                continueOnCapturedContext: commandAttr.ContinueOnCapturedContext);

            Commands.SetTap(control, command);
        }

        private static void AssignCommand(
            BindableObject control, object bindingContext,
            Type controlType, Type bindingContextType,
            CommandAttribute commandAttr)
        {
            if (controlType.GetProperty(commandAttr.CommandName) is PropertyInfo commandProp)
            {
                var method = bindingContextType
                    .GetMethod(commandAttr.CommandDelegateName);

                var canExecuteMethod = bindingContextType
                    .GetMethod(commandAttr.CommandCanExecuteDelegateName);

                var command = new RelayCommand<object>(
                    obj => _ = method.GetParameters().Count() <= 0
                        ? method.Invoke(bindingContext, new object[] { })
                        : method.Invoke(bindingContext, new object[] { obj }),
                    () => canExecuteMethod != default
                        ? (bool)canExecuteMethod.Invoke(bindingContext, new object[0])
                        : true);

                commandProp.SetValue(control, command);
            }
        }

        private static void AssignAsyncCommand(
            BindableObject control, object bindingContext,
            Type controlType, Type bindingContextType,
            AsyncCommandAttribute commandAttr)
        {
            if (controlType.GetProperty(commandAttr.CommandName) is PropertyInfo commandProp)
            {
                var method = bindingContextType
                    .GetMethod(commandAttr.CommandDelegateName);

                var exceptionMethod = bindingContextType
                    .GetMethod(commandAttr.OnException);

                var canExecuteMethod = bindingContextType
                    .GetMethod(commandAttr.CommandCanExecuteDelegateName);

                var command = new AsyncRelayCommand<object>(
                    obj => method.GetParameters().Count() <= 0
                        ? (Task)method.Invoke(bindingContext, new object[] { })
                        : (Task)method.Invoke(bindingContext, new object[] { obj }),
                    canExecute: obj => canExecuteMethod != default
                        ? (bool)canExecuteMethod.Invoke(bindingContext, new object[0])
                        : true,
                    onException: obj => exceptionMethod?.Invoke(bindingContext, new object[] { obj }),
                    continueOnCapturedContext: commandAttr.ContinueOnCapturedContext);

                commandProp.SetValue(control, command);
            }
        }
    }
}