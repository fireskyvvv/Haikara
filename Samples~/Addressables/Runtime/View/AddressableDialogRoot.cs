#if HAIKARA_IS_EXISTS_ADDRESSABLES
using System.Threading.Tasks;
using Haikara.Runtime;
using Haikara.Runtime.Catalog;
using Haikara.Runtime.View;
using Haikara.Samples.Addressables.Runtime.Styles;
using UnityEngine.UIElements;

namespace Haikara.Samples.Addressables.Runtime.View
{
    [HaikaraUI(ReferenceMode = AssetReferenceMode.Custom)]
    public partial class AddressableDialogRoot : HaikaraViewBase
    {
        public override bool IgnoreTemplateContainerLayout => true;
        protected override bool NeedStopPropagation => true;

        public override bool ValidateDataSource(object dataSource)
        {
            return true;
        }

        protected override async Task OnElementLoaded(VisualElement elementRoot)
        {
            await base.OnElementLoaded(elementRoot);

            var styleSheet = await AssetBundleHaikaraDialogRootControl.GetStyleSheet();
            elementRoot.styleSheets.Add(styleSheet);
            elementRoot.AddToClassList(AssetBundleHaikaraDialogRootControl.UsedClassNames.DialogRootBackgroundInit);

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
            targetElement.AddToClassList(AssetBundleHaikaraDialogRootControl.UsedClassNames.DialogRootBackgroundShown);
        }

        public override void ReleaseView()
        {
            base.ReleaseView();
            RuntimeUICatalog.Instance.ReleaseStyleSheet(AssetBundleHaikaraDialogRootControl.UssGuid);
        }
    }
}
#endif