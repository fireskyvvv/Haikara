using UnityEngine;
using UnityEngine.UIElements;

namespace Haikara.Runtime
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class HaikaraManager : MonoBehaviour
    {
#nullable disable
        protected UIDocument uiDocument;
        private HaikaraUIContext _haikaraUIContext;
#nullable restore

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            _haikaraUIContext = new HaikaraUIContext(uiDocument);
            PreprocessInitialize(_haikaraUIContext);
            Initialize(_haikaraUIContext);
        }

        protected virtual void PreprocessInitialize(HaikaraUIContext uiContext)
        {
            
        }

        protected virtual void Initialize(HaikaraUIContext uiContext)
        {
            
        }
    }
}