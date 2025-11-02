using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Haikara.Runtime.ViewModel;
using Unity.Properties;
using UnityEditor.IMGUI.Controls;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public abstract class TreeViewItemViewModel<T> : ViewModelBase where T : TreeViewItemViewModel<T>
    {
        public IList<T> Children { get; }

        protected TreeViewItemViewModel(IList<T>? children = null)
        {
            Children = children ?? new List<T>();
        }
    }

    public abstract class TreeViewItemsSource<T> : ViewModelBase,IList<T> where T : TreeViewItemViewModel<T>
    {
        private readonly IList<T> _items;
        public int Count => _items.Count;
        public bool IsReadOnly => _items.IsReadOnly;

        protected TreeViewItemsSource(IList<T> items)
        {
            _items = items;
        }

        public List<TreeViewItemData<T>> CreateRootItems()
        {
            var id = 0;
            return _items.Select(CreateItemData).ToList();

            TreeViewItemData<T> CreateItemData(T itemViewModel)
            {
                return new TreeViewItemData<T>(
                    id: id++,
                    data: itemViewModel,
                    children: itemViewModel.Children.Select(CreateItemData).ToList()
                );
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }
    }
}