using System.Threading.Tasks;
using Haikara.Runtime;
using Haikara.Runtime.View;
using UnityEngine.UIElements;
using SampleDialogRootControl = Haikara.Samples.Common.Runtime.Dialog.Style.SampleDialogRootControl;

namespace Haikara.Samples.Common.Runtime.Dialog.View
{
    [HaikaraUI]
    public partial class SampleDefaultDialogRoot : HaikaraViewBase
    {
        public override bool IgnoreTemplateContainerLayout => true;
        protected override bool NeedStopPropagation => true;

        public override bool ValidateDataSource(object dataSource)
        {
            return true;
        }

        protected override async Task OnElementLoaded(VisualElement elementRoot)
        {
            var styleSheet = await SampleDialogRootControl.GetStyleSheet();
            elementRoot.styleSheets.Add(styleSheet);
            elementRoot.AddToClassList(SampleDialogRootControl.UsedClassNames.DialogRootBackgroundInit);

            elementRoot.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            var targetElement = evt.target as VisualElement;
            if (targetElement == null)
            {
                return;
            }

            targetElement.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            targetElement.AddToClassList(SampleDialogRootControl.UsedClassNames.DialogRootBackgroundShown);
        }
    }
}