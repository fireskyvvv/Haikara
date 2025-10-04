using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Runtime.ViewModel
{
    public abstract class ViewModelBase : IHaikaraViewModel
    {
        private readonly Dictionary<BindingId, BindablePropertyChangedEventArgs> _propertyChangedEventCaches = new();
        public event EventHandler<BindablePropertyChangedEventArgs>? propertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (!_propertyChangedEventCaches.TryGetValue(property, out var eventArgs))
            {
                eventArgs = new BindablePropertyChangedEventArgs(property);
                _propertyChangedEventCaches.Add(property, eventArgs);
            }

            propertyChanged?.Invoke(this, eventArgs);
        }
    }
}