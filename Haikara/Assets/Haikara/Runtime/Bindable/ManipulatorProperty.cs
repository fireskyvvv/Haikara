using System.Collections.Generic;
using Haikara.Runtime.View;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public class ManipulatorProperty<T> : IElementProperty where T : VisualElement
    {
        public List<T> Elements { get; set; } = new();
        public ElementNameInfo ElementNameInfo { get; }

        private IManipulator Manipulator { get; }

        private ManipulatorProperty(ElementNameInfo elementNameInfo, IManipulator manipulator)
        {
            ElementNameInfo = elementNameInfo;
            Manipulator = manipulator;
        }

        public static ManipulatorProperty<T> Create(ElementNameInfo elementNameInfo, IManipulator manipulator)
        {
            return new ManipulatorProperty<T>(
                elementNameInfo: elementNameInfo,
                manipulator: manipulator
            );
        }

        public void FindElementAndSetBinding(VisualElement elementRoot)
        {
            Elements = ElementNameInfo.Find<T>(elementRoot);

            foreach (var element in Elements)
            {
                if (element is Button button && Manipulator is Clickable clickable)
                {
                    button.clickable = clickable;
                }
                else
                {
                    element.AddManipulator(Manipulator);
                }
            }
        }
    }
}