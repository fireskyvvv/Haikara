#if HAIKARA_IS_EXISTS_ADDRESSABLES
using System;
using System.Collections.Generic;
using Haikara.Runtime.Catalog;
using Haikara.Runtime.Util;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace Haikara.Editor.AddressablesExtensions.AddressablesMaker
{
    internal static class AddressablesGroupMaker
    {
        public static void Build()
        {
            var builderSettings = HaikaraAddressablesGroupMakerSettings.instance;
            var haikaraUxmlGroupName = builderSettings.uxmlGroupName;
            var haikaraUssGroupName = builderSettings.ussGroupName;

            if (string.IsNullOrWhiteSpace(haikaraUxmlGroupName) || string.IsNullOrWhiteSpace(haikaraUssGroupName))
            {
                throw new Exception(
                    "[Haikara Addressables Group Maker]Invalid uxmlGroupName or ussGroupName. " +
                    $"uxmlGroupName: `{haikaraUxmlGroupName}`, ussGroupName: `{haikaraUssGroupName}`"
                );
            }

            var uiCatalog = UICatalog.Instance;
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);

            RegenerateUIAssetAddressableGroups(
                groupName: haikaraUxmlGroupName,
                settings,
                uiCatalog.UxmlAssets.CustomUIInfoList
            );

            RegenerateUIAssetAddressableGroups(
                groupName: haikaraUssGroupName,
                settings,
                uiCatalog.StyleAssets.CustomUIInfoList
            );

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }

        private static void RegenerateUIAssetAddressableGroups<TAsset>(
            string groupName,
            AddressableAssetSettings settings,
            List<CustomUIInfo<TAsset>> uiAssetInfoList
        ) where TAsset : UnityEngine.Object
        {
            var group = settings.FindGroup(groupName);
            if (group == null)
            {
                group = settings.CreateGroup(groupName,
                    setAsDefaultGroup: false,
                    readOnly: false,
                    postEvent: true,
                    schemasToCopy: null,
                    typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema)
                );
            }

            var schema = group.GetSchema<BundledAssetGroupSchema>();
            if (schema.BundleMode != BundledAssetGroupSchema.BundlePackingMode.PackSeparately)
            {
                UnityEngine.Debug.LogWarning(
                    HaikaraLogUtil.GetMessage(
                        $"The packing mode '{BundledAssetGroupSchema.BundlePackingMode.PackSeparately}' is recommended."
                    )
                );
            }

            foreach (var uiAssetInfo in uiAssetInfoList)
            {
                var entry = group.Settings.CreateOrMoveEntry(
                    guid: uiAssetInfo.Id,
                    targetParent: group,
                    readOnly: false,
                    postEvent: false
                );
                entry.SetAddress(uiAssetInfo.Id);
            }
        }
    }
}
#endif