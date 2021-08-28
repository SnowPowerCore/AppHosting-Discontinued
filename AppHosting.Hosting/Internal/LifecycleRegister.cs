using AppHosting.Abstractions.Interfaces;
using System;
using System.Collections.Generic;

namespace AppHosting.Hosting.Internal
{
    internal class LifecycleRegister : ILifecycleRegister
    {
        private readonly HashSet<Action> _callbacks = new();

        public void Register(Action callback) =>
            _callbacks.Add(callback);

        public void Notify()
        {
            foreach (var callback in _callbacks)
                callback.Invoke();
        }
    }
}