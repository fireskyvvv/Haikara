using System.Collections.Generic;
using Haikara.Runtime.View;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public abstract class BindablePropertyBase<T> : IBindableProperty where T : VisualElement
    {
        public abstract ElementNameInfo ElementNameInfo { get; }
        public List<T> Elements { get; set; } = new();
        private object? _dataSource;

        public object? DataSource
        {
            get { return _dataSource; }
            set
            {
                var previousValue = _dataSource;
                var newValue = value;

                if (previousValue != null && newValue != null)
                {
                    if (ReferenceEquals(previousValue, newValue))
                    {
                        return;
                    }
                }

                _dataSource = newValue;
                OnDataSourceChanged(previousValue, newValue);
            }
        }

        public abstract void FindElementAndSetBinding(VisualElement elementRoot);

        private void OnDataSourceChanged(object? previous, object? newValue)
        {
            if (Elements.Count == 0)
            {
                return;
            }

            foreach (var element in Elements)
            {
                ElementDataSourceChanging(element, previous, newValue);
            }
        }

        protected virtual void ElementDataSourceChanging(T element, object? previous, object? newValue)
        {
            element.dataSource = newValue;
        }
    }
}