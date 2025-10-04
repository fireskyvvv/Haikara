using System;
using Haikara.Runtime.View;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public class EnumFieldValueProperty : BindablePropertyBase<EnumField>
    {
        private PropertyPath DataSourcePath { get; }
        private BindingMode BindingMode { get; }
        private BindingUpdateTrigger UpdateTrigger { get; }
        public override ElementNameInfo ElementNameInfo { get; }
        private readonly Enum _defaultValue;
        private readonly bool _includeObsoleteValues;

        public override void FindElementAndSetBinding(VisualElement elementRoot)
        {
            Elements = ElementNameInfo.Find<EnumField>(elementRoot);
            foreach (var element in Elements)
            {
                // EnumField must be initialized with a specific enum type.
                element.Init(_defaultValue, _includeObsoleteValues);
                element.SetBinding(
                    bindingId: PropertyPath.FromName(nameof(EnumField.value)),
                    binding: new DataBinding()
                    {
                        bindingMode = BindingMode,
                        dataSourcePath = DataSourcePath,
                        updateTrigger = UpdateTrigger,
                    }
                );
            }
        }

        private EnumFieldValueProperty(
            Enum defaultValue,
            PropertyPath dataSourcePath,
            ElementNameInfo elementNameInfo,
            BindingMode bindingMode,
            BindingUpdateTrigger updateTrigger,
            bool includeObsoleteValues
        )
        {
            _defaultValue = defaultValue;
            DataSourcePath = dataSourcePath;
            ElementNameInfo = elementNameInfo;
            BindingMode = bindingMode;
            UpdateTrigger = updateTrigger;
            _includeObsoleteValues = includeObsoleteValues;
        }

        public static EnumFieldValueProperty Create(
            Enum defaultValue,
            PropertyPath dataSourcePath,
            ElementNameInfo elementNameInfo,
            BindingMode bindingMode = BindingMode.ToTarget,
            BindingUpdateTrigger updateTrigger = BindingUpdateTrigger.OnSourceChanged,
            bool includeObsoleteValues = false
        )
        {
            return new EnumFieldValueProperty(
                defaultValue: defaultValue,
                dataSourcePath: dataSourcePath,
                elementNameInfo: elementNameInfo,
                bindingMode: bindingMode,
                updateTrigger: updateTrigger,
                includeObsoleteValues: includeObsoleteValues
            );
        }
    }
}