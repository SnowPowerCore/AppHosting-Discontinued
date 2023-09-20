using AppHosting.Xamarin.Forms.Attributes;
using AppHosting.Xamarin.Forms.Shared.Utils.Touch;
using AppHosting.Xamarin.Forms.Utils.Commands;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

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

        public static Element AddAttachedLongPressCommands(
            this Element xfElement,
            AttachedLongPressCommandAttribute[] commandAttrs,
            object differentBindingContext = default)
        {
            foreach (var commandAttr in commandAttrs)
            {
                if (string.IsNullOrEmpty(commandAttr.CommandDelegateName))
                    continue;

                var desiredControl = xfElement.GetControlData(commandAttr.ControlName,
                    out _, out var bindingContextType);

                bindingContextType ??= differentBindingContext?.GetType();

                AssignAttachedLongPressCommandParameter(
                    desiredControl, xfElement.BindingContext ?? differentBindingContext,
                    commandAttr);

                AssignAttachedLongPressCommand(
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

        public static Element AddAttachedAsyncLongPressCommands(
            this Element xfElement,
            AttachedAsyncLongPressCommandAttribute[] commandAttrs,
            object differentBindingContext = default)
        {
            foreach (var commandAttr in commandAttrs)
            {
                if (string.IsNullOrEmpty(commandAttr.CommandDelegateName))
                    continue;

                var desiredControl = xfElement.GetControlData(commandAttr.ControlName,
                    out _, out var bindingContextType);

                bindingContextType ??= differentBindingContext?.GetType();

                AssignAttachedLongPressCommandParameter(
                    desiredControl, xfElement.BindingContext ?? differentBindingContext,
                    commandAttr);

                AssignAttachedAsyncLongPressCommand(
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
                    desiredControl, xfElement.BindingContext ?? differentBindingContext, desiredControlType, commandAttr);

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
                    desiredControl, xfElement.BindingContext ?? differentBindingContext, desiredControlType, commandAttr);

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
                var binding = CreateCommandParameterBinding(control, bindingContext, commandAttr);
                if (binding is not default(Binding))
                    control.SetBinding(TouchEffect.CommandParameterProperty, binding);
            }
        }

        private static void AssignAttachedLongPressCommandParameter(
            BindableObject control, object bindingContext, CommandAttribute commandAttr)
        {
            if (!string.IsNullOrEmpty(commandAttr.CommandObjectName))
            {
                var binding = CreateCommandParameterBinding(control, bindingContext, commandAttr);
                if (binding is not default(Binding))
                    control.SetBinding(TouchEffect.LongPressCommandParameterProperty, binding);
            }
        }

        private static void AssignCommandParameter(
            BindableObject control, object bindingContext, Type controlType, CommandAttribute commandAttr)
        {
            if (controlType.GetProperty("CommandParameter") is not default(PropertyInfo))
            {
                var binding = CreateCommandParameterBinding(control, bindingContext, commandAttr);
                if (binding is not default(Binding))
                    control.SetBinding(
                        (BindableProperty)controlType.GetField("CommandParameterProperty").GetValue(control),
                        binding);
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

            var command = CreateRelayCommand(bindingContext, method, canExecuteMethod);

            TouchEffect.SetCommand(control, command);
            TouchEffect.SetNativeAnimation(control, commandAttr.NativeAnimation);
            TouchEffect.SetHoveredAnimationDuration(control, commandAttr.HoveredAnimationDuration);
            TouchEffect.SetNormalAnimationDuration(control, commandAttr.NormalAnimationDuration);
        }

        private static void AssignAttachedLongPressCommand(
            BindableObject control, object bindingContext,
            Type bindingContextType,
            AttachedLongPressCommandAttribute commandAttr)
        {
            var method = bindingContextType
                .GetMethod(commandAttr.CommandDelegateName);

            var canExecuteMethod = bindingContextType
                .GetMethod(commandAttr.CommandCanExecuteDelegateName);

            var command = CreateRelayCommand(bindingContext, method, canExecuteMethod);

            TouchEffect.SetLongPressCommand(control, command);
            TouchEffect.SetLongPressDuration(control, commandAttr.LongPressDuration);
            TouchEffect.SetNativeAnimation(control, commandAttr.NativeAnimation);
            TouchEffect.SetHoveredAnimationDuration(control, commandAttr.HoveredAnimationDuration);
            TouchEffect.SetNormalAnimationDuration(control, commandAttr.NormalAnimationDuration);
            TouchEffect.SetPressedAnimationDuration(control, commandAttr.PressedAnimationDuration);
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

            var command = CreateAsyncRelayCommand(bindingContext, method, canExecuteMethod, exceptionMethod, commandAttr.ContinueOnCapturedContext);

            TouchEffect.SetCommand(control, command);
            TouchEffect.SetNativeAnimation(control, commandAttr.NativeAnimation);
            TouchEffect.SetHoveredAnimationDuration(control, commandAttr.HoveredAnimationDuration);
            TouchEffect.SetNormalAnimationDuration(control, commandAttr.NormalAnimationDuration);
        }

        private static void AssignAttachedAsyncLongPressCommand(
            BindableObject control, object bindingContext,
            Type bindingContextType,
            AttachedAsyncLongPressCommandAttribute commandAttr)
        {
            var method = bindingContextType
                .GetMethod(commandAttr.CommandDelegateName);

            var exceptionMethod = bindingContextType
                .GetMethod(commandAttr.OnException);

            var canExecuteMethod = bindingContextType
                .GetMethod(commandAttr.CommandCanExecuteDelegateName);

            var command = CreateAsyncRelayCommand(bindingContext, method, canExecuteMethod, exceptionMethod, commandAttr.ContinueOnCapturedContext);

            TouchEffect.SetLongPressCommand(control, command);
            TouchEffect.SetNativeAnimation(control, commandAttr.NativeAnimation);
            TouchEffect.SetHoveredAnimationDuration(control, commandAttr.HoveredAnimationDuration);
            TouchEffect.SetNormalAnimationDuration(control, commandAttr.NormalAnimationDuration);
            TouchEffect.SetPressedAnimationDuration(control, commandAttr.PressedAnimationDuration);
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
                RelayCommand<object> command = CreateRelayCommand(bindingContext, method, canExecuteMethod);

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

                var command = CreateAsyncRelayCommand(bindingContext, method, canExecuteMethod, exceptionMethod, commandAttr.ContinueOnCapturedContext);

                commandProp.SetValue(control, command);
            }
        }

        private static Binding CreateCommandParameterBinding(BindableObject control, object bindingContext, CommandAttribute commandAttr)
        {
            if (string.IsNullOrEmpty(commandAttr.CommandObjectName))
                return default;

            if (commandAttr.CommandObjectName.Equals(".", StringComparison.OrdinalIgnoreCase))
                return new Binding(
                    Binding.SelfPath,
                    commandAttr.ParameterBindingMode,
                    source: control.BindingContext);

            return new Binding(
                commandAttr.CommandObjectName,
                commandAttr.ParameterBindingMode,
                source: bindingContext);
        }

        private static RelayCommand<object> CreateRelayCommand(object bindingContext, MethodInfo method, MethodInfo canExecuteMethod) =>
            new(obj => _ = method.GetParameters().Length <= 0
                    ? method.Invoke(bindingContext, new object[] { })
                    : method.Invoke(bindingContext, new object[] { obj }),
                obj =>
                {
                    if (canExecuteMethod is not default(MethodInfo))
                    {
                        return canExecuteMethod.GetParameters().Length <= 0
                            ? (bool)canExecuteMethod.Invoke(bindingContext, new object[0])
                            : (bool)canExecuteMethod.Invoke(bindingContext, new object[] { obj });
                    }
                    return true;
                });

        private static AsyncRelayCommand<object> CreateAsyncRelayCommand(object bindingContext, MethodInfo method, MethodInfo canExecuteMethod, MethodInfo exceptionMethod, bool continueOnCapturedContext) =>
            new(obj => method.GetParameters().Length <= 0
                    ? (Task)method.Invoke(bindingContext, new object[] { })
                    : (Task)method.Invoke(bindingContext, new object[] { obj }),
                obj =>
                {
                    if (canExecuteMethod is not default(MethodInfo))
                    {
                        return canExecuteMethod.GetParameters().Length <= 0
                            ? (bool)canExecuteMethod.Invoke(bindingContext, new object[0])
                            : (bool)canExecuteMethod.Invoke(bindingContext, new object[] { obj });
                    }
                    return true;
                },
                onException: e => exceptionMethod?.Invoke(bindingContext, new object[] { e }),
                continueOnCapturedContext: continueOnCapturedContext);
    }
}