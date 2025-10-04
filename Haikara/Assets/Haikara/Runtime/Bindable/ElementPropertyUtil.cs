using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public static class ElementPropertyUtil
    {
        /// <summary>
        /// Sort bindings by priority to ensure the correct execution order.
        /// </summary>
        /// <returns></returns>
        public static void SortElementPropertiesByPriority(this List<IElementProperty> elementProperties)
        {
            elementProperties.Sort(
                (left, right) => GetPriority(left) - GetPriority(right)
            );
        }

        /// <summary>
        /// Gets the priority from the bindingId and other properties.
        /// </summary>
        private static int GetPriority(this IElementProperty elementProperty)
        {
            // In DropdownField, the 'choices' has the highest priority.
            if (elementProperty is BindableProperty<DropdownField> dropdownProperty)
            {
                if (dropdownProperty.BindingId == (BindingId)PropertyPath.FromName(nameof(DropdownField.choices)))
                {
                    return 0;
                }
            }

            // In RadioButtonGroup, the 'choices' has the highest priority.
            if (elementProperty is BindableProperty<RadioButtonGroup> radioButtonGroupProperty)
            {
                if (radioButtonGroupProperty.BindingId ==
                    (BindingId)PropertyPath.FromName(nameof(RadioButtonGroup.choices)))
                {
                    return 0;
                }
            }

            return 100;
        }
    }
}