using System;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Utils.Wrappers
{
    public class TypeWrapper
    {
        public Type Type { get; set; }

        public BindableObject Parent { get; set; }
    }
}