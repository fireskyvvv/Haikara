using Haikara.Runtime.View;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public interface IElementProperty
    {
        public ElementNameInfo ElementNameInfo { get; }

        public void FindElementAndSetBinding(VisualElement elementRoot);
    }
}