#if HAIKARA_IS_EXISTS_ADDRESSABLES
using Haikara.Editor.AddressablesExtensions.AddressablesMaker.View;
using UnityEditor;
using UnityEngine;

namespace Haikara.Editor.AddressablesExtensions.AddressablesMaker
{
    internal class HaikaraAddressablesGroupMakerWindow : EditorWindow
    {
        [MenuItem("Haikara/Addressables/GroupMaker Window")]
        private static void Init()
        {
            var w = GetWindow<HaikaraAddressablesGroupMakerWindow>();
            w.titleContent = new GUIContent("Haikara Addressables Group Maker");
            w.minSize = new Vector2(550, 300);
        }

        private async void CreateGUI()
        {
            var view = new AddressablesGroupMakerLayout();
            await view.LoadAndAddToAsync(rootVisualElement);
            view.SetDataSource(new HaikaraAddressablesGroupMakerViewModel());
        }
    }
}

#endif