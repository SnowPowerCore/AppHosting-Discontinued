using AppHosting.Xamarin.Forms.Utils.Wrappers;
using System;
using System.Collections.Generic;

namespace AppHosting.Xamarin.Forms.Utils.DataTemplate
{
    public class MemorizedDataTemplate : global::Xamarin.Forms.DataTemplate
    {
        private static readonly Dictionary<Type, object> _createdContents = new();

        public MemorizedDataTemplate() { }

        public MemorizedDataTemplate(TypeWrapper typeWrapper) : base(() =>
        {
            var type = typeWrapper.Type;
            if (_createdContents.ContainsKey(type))
                return _createdContents[type];

            var data = Activator.CreateInstance(type);
            _createdContents.Add(type, data);
            return data;
        })
        { }
    }
}