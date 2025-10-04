using System;
using Haikara.Runtime.View;
using Haikara.Runtime.ViewModel;
using UnityEngine.UIElements;

namespace Haikara.Runtime
{
    public class HaikaraUIContext
    {
        private readonly UIDocument _uiDocument;

        public HaikaraUIContext(UIDocument uiDocument)
        {
            _uiDocument = uiDocument;
        }

        public VisualElement GetUiDocumentRootElement()
        {
            return _uiDocument.rootVisualElement;
        }
    }
}