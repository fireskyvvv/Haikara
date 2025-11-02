using System.Collections.Generic;
using Haikara.Runtime.ViewModel;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public abstract class TreeViewEntryViewModel<T> : ViewModelBase where T : TreeViewEntryViewModel<T>
    {
        public List<T> Children = new();

        public TreeViewItemData<T> CreateTreeViewItemData(ref int id)
        {
            var children = new List<TreeViewItemData<T>>(Children.Count);
            foreach (var child in Children)
            {
                children.Add(child.CreateTreeViewItemData(ref id));
            }
            
            return new TreeViewItemData<T>(id++, (T)this, children);   
        }
    }
}