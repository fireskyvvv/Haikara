#if HAIKARA_IS_EXISTS_ADDRESSABLES
using UnityEditor;
using UnityEngine;

namespace Haikara.Editor.AddressablesExtensions.AddressablesMaker
{
    [FilePath("HaikaraConfigs/addressablesConfig.dat", FilePathAttribute.Location.ProjectFolder)]
    internal class HaikaraAddressablesGroupMakerSettings : ScriptableSingleton<HaikaraAddressablesGroupMakerSettings>
    {
        [SerializeField] public string uxmlGroupName = "HaikaraUxml";
        [SerializeField] public string ussGroupName = "HaikaraUSS";

        public void SaveAsText()
        {
            Save(saveAsText: true);
        }
    }
}
#endif