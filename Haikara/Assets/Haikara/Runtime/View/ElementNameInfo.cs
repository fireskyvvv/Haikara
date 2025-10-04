using System.Collections.Generic;
using Haikara.Runtime.Util;
using UnityEngine.UIElements;

namespace Haikara.Runtime.View
{
    public readonly struct ElementNameInfo
    {
        public enum ElementFindType
        {
            First,
            Index,
            All
        }

        public ElementName ElementName { get; }
        private int Index { get; }
        private ElementFindType FindType { get; }

        /// <summary>
        /// An index of -1 indicates that this has an ElementName.
        /// </summary>
        public ElementNameInfo(
            ElementName elementName,
            int index = -1,
            ElementFindType findType = ElementFindType.First
        )
        {
            ElementName = elementName;
            Index = index;
            FindType = findType;
        }

        public static implicit operator ElementNameInfo(string value)
        {
            return new ElementNameInfo(elementName: value);
        }

        public List<T> Find<T>(VisualElement elementRoot) where T : VisualElement
        {
            var query = elementRoot.Query<T>(ElementName).Build();
            var result = new List<T>();

            if (FindType == ElementFindType.First)
            {
                var foundElement = query.First();
                if (foundElement != null)
                {
                    result.Add(foundElement);
                }
            }
            else if (FindType == ElementFindType.Index)
            {
                var foundElement = Index == -1
                    ? query.First()
                    : query.AtIndex(Index);
                if (foundElement != null)
                {
                    result.Add(foundElement);
                }
            }
            else if (FindType == ElementFindType.All)
            {
                var foundElements = query.ToList();
                if (foundElements != null)
                {
                    result.AddRange(foundElements);
                }
            }

            if (result.Count == 0)
            {
                UnityEngine.Debug.LogError(
                    HaikaraLogUtil.GetMessage(
                        $"ElementName is not found. Name: `{ElementName.Name}`, Type: `{typeof(T)}`")
                );
            }

            return result;
        }

        public override string ToString()
        {
            return ElementName;
        }
    }
}