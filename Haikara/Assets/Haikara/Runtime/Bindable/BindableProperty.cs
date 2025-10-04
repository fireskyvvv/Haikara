using Haikara.Runtime.View;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public class BindableProperty<T> : BindablePropertyBase<T> where T : VisualElement
    {
        public BindingId BindingId { get; }
        private PropertyPath DataSourcePath { get; }
        private BindingMode BindingMode { get; }
        private BindingUpdateTrigger UpdateTrigger { get; }
        public override ElementNameInfo ElementNameInfo { get; }

        public override void FindElementAndSetBinding(VisualElement elementRoot)
        {
            Elements = ElementNameInfo.Find<T>(elementRoot);
            foreach (var element in Elements)
            {
                element?.SetBinding(
                    bindingId: BindingId,
                    binding: new DataBinding()
                    {
                        bindingMode = BindingMode,
                        dataSourcePath = DataSourcePath,
                        updateTrigger = UpdateTrigger,
                    }
                );
            }
        }

        protected BindableProperty(
            BindingId bindingId,
            PropertyPath dataSourcePath,
            ElementNameInfo elementNameInfo,
            BindingMode bindingMode,
            BindingUpdateTrigger updateTrigger)
        {
            BindingId = bindingId;
            DataSourcePath = dataSourcePath;
            ElementNameInfo = elementNameInfo;
            BindingMode = bindingMode;
            UpdateTrigger = updateTrigger;
        }

        public static BindableProperty<T> Create(
            BindingId bindingId,
            PropertyPath dataSourcePath,
            ElementNameInfo elementNameInfo,
            BindingMode bindingMode = BindingMode.ToTarget,
            BindingUpdateTrigger updateTrigger = BindingUpdateTrigger.OnSourceChanged
        )
        {
            return new BindableProperty<T>(
                bindingId: bindingId,
                dataSourcePath: dataSourcePath,
                elementNameInfo: elementNameInfo,
                bindingMode: bindingMode,
                updateTrigger: updateTrigger
            );
        }
    }
}