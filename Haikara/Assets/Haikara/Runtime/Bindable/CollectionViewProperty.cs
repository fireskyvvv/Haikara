using Haikara.Runtime.ViewModel;
using Haikara.Runtime.View;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public abstract class CollectionViewProperty<TVerticalCollection> : BindablePropertyBase<TVerticalCollection>
        where TVerticalCollection : BaseVerticalCollectionView
    {
        private PropertyPath ItemsSourcePath { get; }
        private BindingMode BindingMode { get; }
        private BindingUpdateTrigger UpdateTrigger { get; }
        public override ElementNameInfo ElementNameInfo { get; }

        public override void FindElementAndSetBinding(VisualElement elementRoot)
        {
            Elements = ElementNameInfo.Find<TVerticalCollection>(elementRoot);
            foreach (var element in Elements)
            {
                if (element == null)
                {
                    continue;
                }

                element.SetBinding(
                    bindingId: PropertyPath.FromName(nameof(BaseVerticalCollectionView.itemsSource)),
                    binding: new DataBinding()
                    {
                        bindingMode = BindingMode,
                        dataSourcePath = ItemsSourcePath,
                        updateTrigger = UpdateTrigger,
                    }
                );
                SetBinding(element);
            }
        }

        protected abstract void SetBinding(TVerticalCollection element);

        protected CollectionViewProperty(
            PropertyPath itemsSourcePath,
            ElementNameInfo elementNameInfo,
            BindingMode bindingMode = BindingMode.ToTarget,
            BindingUpdateTrigger updateTrigger = BindingUpdateTrigger.OnSourceChanged)
        {
            ItemsSourcePath = itemsSourcePath;
            ElementNameInfo = elementNameInfo;
            BindingMode = bindingMode;
            UpdateTrigger = updateTrigger;
        }
    }
}