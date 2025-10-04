using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Haikara.Editor.Utils;
using Haikara.Runtime;
using Haikara.Runtime.Catalog;
using Haikara.Runtime.Style;
using Haikara.Runtime.Util;
using Haikara.Runtime.View;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Compilation;

namespace Haikara.Editor.Catalog
{
    public static class UICatalogGenerator
    {
        [DidReloadScripts]
        private static void OnAssemblyReloadFinished()
        {
            Regenerate();
        }

        [MenuItem("Haikara/Regenerate UI Catalog")]
        private static void RegenerateManual()
        {
            Regenerate(needClear: true);
        }

        public static void Regenerate(bool needClear = false, bool isPreprocessBuild = false)
        {
            try
            {
                var currentCatalog = UnityEngine.Resources.Load<UICatalog>(UICatalog.AssetName);
                if (currentCatalog == null)
                {
                    var resourcesDirectory = Path.Combine("Assets", "Resources");
                    if (!Directory.Exists(resourcesDirectory))
                    {
                        Directory.CreateDirectory(resourcesDirectory);
                    }

                    currentCatalog = ScriptableObjectCreateUtil.Create<UICatalog>(
                        createFolderPath: resourcesDirectory,
                        assetName: UICatalog.AssetName,
                        focusAfterCreated: false
                    );
                }

                if (needClear)
                {
                    currentCatalog.UxmlAssets.AssetPathUIInfoList.Clear();
                    currentCatalog.UxmlAssets.ResourceUIInfoList.Clear();
                    currentCatalog.UxmlAssets.CustomUIInfoList.Clear();

                    currentCatalog.StyleAssets.CustomUIInfoList.Clear();
                    currentCatalog.StyleAssets.CustomUIInfoList.Clear();
                    currentCatalog.StyleAssets.CustomUIInfoList.Clear();
                }

                var catalogSources = CatalogSourceGenerator.GenerateCatalogSources().ToList();
                var uxmlFileGuids = catalogSources
                    .SelectMany(catalogSource => catalogSource.UxmlFileGuids)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Where(x =>
                    {
                        // During the build process, skip the GUIDs of UXML files located in EditorOnly assemblies.
                        if (!isPreprocessBuild)
                        {
                            return true;
                        }

                        return !IsInEditorOnlyAssembly(x);
                    });

                var styleFileGuids = catalogSources
                    .SelectMany(catalogSource => catalogSource.UssFileGuids)
                    .Where(x =>
                    {
                        // During the build process, skip the GUIDs of USS files located in EditorOnly assemblies.
                        if (!isPreprocessBuild)
                        {
                            return true;
                        }

                        return !IsInEditorOnlyAssembly(x);
                    });

                var uxmlGuidAndReferenceModePairs =
                    GetUIAssetGuidAndReferenceModePairs<IHaikaraView>(uxmlFileGuids.ToList());
                var ussGuidAndReferenceModePairs =
                    GetUIAssetGuidAndReferenceModePairs<IHaikaraStyle>(styleFileGuids.ToList());

                RegenerateCatalogAsset(
                    currentCatalog,
                    uxmlGuidAndReferenceModePairs: uxmlGuidAndReferenceModePairs,
                    ussGuidAndReferenceModePairs: ussGuidAndReferenceModePairs,
                    isPreprocessBuild
                );
                EditorUtility.SetDirty(currentCatalog);
                AssetDatabase.SaveAssetIfDirty(currentCatalog);
                AssetDatabase.Refresh();

                UnityEngine.Resources.UnloadAsset(currentCatalog);

                UnityEngine.Debug.Log(HaikaraLogUtil.GetMessage("UICatalog Regenerated."));
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
                throw;
            }
        }

        private static void RegenerateCatalogAsset(
            UICatalog currentCatalog,
            IEnumerable<(string uxmlGuid, AssetReferenceMode referenceMode)> uxmlGuidAndReferenceModePairs,
            IEnumerable<(string uxmlGuid, AssetReferenceMode referenceMode)> ussGuidAndReferenceModePairs,
            bool isPreprocessBuild
        )
        {
            UpdateUiAssets(uxmlGuidAndReferenceModePairs, currentCatalog.UxmlAssets);
            UpdateUiAssets(ussGuidAndReferenceModePairs, currentCatalog.StyleAssets);

            // During the build process, clear the AssetPathUIInfoList.
            if (isPreprocessBuild)
            {
                currentCatalog.UxmlAssets.AssetPathUIInfoList.Clear();
                currentCatalog.StyleAssets.AssetPathUIInfoList.Clear();
            }

            return;

            void UpdateUiAssets<T>(
                IEnumerable<(string assetGuid, AssetReferenceMode referenceMode)> assetGuidAndReferenceModePairs,
                UIAssets<T> currentUiAssets
            ) where T : UnityEngine.Object
            {
                var guidAndReferenceModeGroups = assetGuidAndReferenceModePairs.GroupBy(x => x.referenceMode);
                var assetPathModeGuidList = new List<string>();
                var resourceModeGuidList = new List<string>();
                var assetBundleModeGuidList = new List<string>();

                // It must be detected even if it is empty.
                foreach (var guidAndReferenceModeGroup in guidAndReferenceModeGroups)
                {
                    switch (guidAndReferenceModeGroup.Key)
                    {
                        case AssetReferenceMode.AssetPath:
                            assetPathModeGuidList = guidAndReferenceModeGroup
                                .Select(x => x.assetGuid)
                                .ToList();
                            break;
                        case AssetReferenceMode.Resource:
                            resourceModeGuidList = guidAndReferenceModeGroup.Select(x => x.assetGuid).ToList();
                            break;
                        case AssetReferenceMode.Custom:
                            assetBundleModeGuidList =
                                guidAndReferenceModeGroup.Select(x => x.assetGuid).ToList();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // During the build process, do not generate UIAssetInfo for AssetPaths.
                if (!isPreprocessBuild)
                {
                    UpdateUIAssetInfo(assetPathModeGuidList, currentUiAssets, AssetReferenceMode.AssetPath);
                }

                UpdateUIAssetInfo(resourceModeGuidList, currentUiAssets, AssetReferenceMode.Resource);
                UpdateUIAssetInfo(assetBundleModeGuidList, currentUiAssets, AssetReferenceMode.Custom);
            }

            void UpdateUIAssetInfo<T>(
                List<string> guidList,
                UIAssets<T> currentUiAssets,
                AssetReferenceMode referenceMode
            ) where T : UnityEngine.Object
            {
                List<string> currentGuidList;

                // Remove UIAssets with IDs contained in the guidList, and then create new ones for any missing entries.
                switch (referenceMode)
                {
                    case AssetReferenceMode.AssetPath:

                        currentGuidList = currentUiAssets.AssetPathUIInfoList.Select(x => x.Id).ToList();
                        var currentGuidToPath =
                            currentUiAssets.AssetPathUIInfoList.ToDictionary(x => x.Id, x => x.AssetPath);

                        currentUiAssets.AssetPathUIInfoList.RemoveAll(x =>
                        {
                            // Remove any currently registered IDs that are not found in the detected GuidList.
                            if (!guidList.Contains(x.Id))
                            {
                                return true;
                            }

                            // Remove the item if its AssetPath has changed.
                            if (currentGuidToPath.TryGetValue(x.Id, out var currentRegisteredAssetPath))
                            {
                                var currentAssetPath = AssetDatabase.GUIDToAssetPath(x.Id);
                                if (currentAssetPath != currentRegisteredAssetPath)
                                {
                                    return true;
                                }
                            }

                            return false;
                        });

                        currentUiAssets.AssetPathUIInfoList.AddRange(
                            guidList.Except(currentGuidList)
                                .Select(guid =>
                                {
                                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);

                                    return new AssetPathUIInfo<T>(guid, assetPath);
                                })
                        );
                        break;
                    case AssetReferenceMode.Resource:
                        currentGuidList = currentUiAssets.ResourceUIInfoList.Select(x => x.Id).ToList();
                        var currentGuidToRegisteredAsset =
                            currentUiAssets.ResourceUIInfoList.ToDictionary(x => x.Id, x => x.Asset);
                        currentUiAssets.ResourceUIInfoList.RemoveAll(x =>
                        {
                            // Remove any currently registered IDs that are not found in the detected GuidList.
                            if (!guidList.Contains(x.Id))
                            {
                                return true;
                            }

                            // Remove the item if its AssetPath has changed.
                            if (currentGuidToRegisteredAsset.TryGetValue(x.Id, out var currentRegisteredAsset))
                            {
                                if (AssetDatabase.GetAssetPath(currentRegisteredAsset) !=
                                    AssetDatabase.GUIDToAssetPath(x.Id))
                                {
                                    return true;
                                }
                            }

                            return false;
                        });

                        currentUiAssets.ResourceUIInfoList.AddRange(
                            guidList.Except(currentGuidList)
                                .Select(guid =>
                                {
                                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                                    var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                                    return new ResourceUIInfo<T>(guid, asset);
                                })
                        );
                        break;
                    case AssetReferenceMode.Custom:
                        currentGuidList = currentUiAssets.CustomUIInfoList.Select(x => x.Id).ToList();
                        currentUiAssets.CustomUIInfoList.RemoveAll(x => !guidList.Contains(x.Id));

                        currentUiAssets.CustomUIInfoList.AddRange(
                            guidList
                                .Except(currentGuidList)
                                .Select(guid => new CustomUIInfo<T>(guid))
                        );
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(referenceMode), referenceMode, null);
                }
            }
        }

        private static IEnumerable<(string uiAssetGuid, AssetReferenceMode referenceMode)>
            GetUIAssetGuidAndReferenceModePairs<THaikaraUI>(List<string> targetAssetGuids)
            where THaikaraUI : IHaikaraUI
        {
            var haikaraUITypes = TypeCache.GetTypesWithAttribute<HaikaraUIAttribute>();
            foreach (var haikaraUIType in haikaraUITypes)
            {
                // Skip items that aren't decorated with the HaikaraUIAttribute.
                var attribute = haikaraUIType.GetCustomAttribute<HaikaraUIAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                // Skip type that do not implement THaikaraUI.
                if (!typeof(THaikaraUI).IsAssignableFrom(haikaraUIType))
                {
                    continue;
                }

                string guidFieldName;
                if (typeof(THaikaraUI) == typeof(IHaikaraView))
                {
                    guidFieldName = "UxmlGuid";
                }
                else if (typeof(THaikaraUI) == typeof(IHaikaraStyle))
                {
                    guidFieldName = "UssGuid";
                }
                else
                {
                    throw new Exception($"Invalid HaikaraUIType. Type: {typeof(THaikaraUI)}");
                }

                var field = haikaraUIType.GetField(guidFieldName, BindingFlags.Public | BindingFlags.Static);
                if (field == null)
                {
                    continue;
                }

                var guid = (string)field.GetRawConstantValue();
                if (targetAssetGuids.Contains(guid))
                {
                    yield return (guid, attribute.ReferenceMode);
                }
            }
        }

        private static bool IsInEditorOnlyAssembly(string guid)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrWhiteSpace(assetPath))
            {
                return false;
            }

            if (!AssemblyDefinitionFinder.TryGetBelongingAssemblyDefinitionAssetPath(
                    assetPath: assetPath,
                    out var asmdefAssetPath))
            {
                return false;
            }

            var asmdefInfo = AssemblyDefinitionFinder.GetAsmdefInfo(asmdefAssetPath);

            var allAssemblies = CompilationPipeline.GetAssemblies();
            var targetAssembly = allAssemblies.FirstOrDefault(x => x.name == asmdefInfo.name);
            if (targetAssembly == null)
            {
                return false;
            }

            if ((targetAssembly.flags & AssemblyFlags.EditorAssembly) == 0)
            {
                return false;
            }

            UnityEngine.Debug.Log(
                HaikaraLogUtil.GetMessage($"AssetPath: {assetPath} is EditorOnlyUIAsset. Skip build")
            );
            return true;
        }
    }
}