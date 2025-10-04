using System.Collections.Generic;
using System.IO;
using System.Linq;
using Haikara.Editor.Utils;
using UnityEditor;

namespace Haikara.Editor.Catalog
{
    public static class CatalogSourceGenerator
    {
        public static IEnumerable<CatalogSource> GenerateCatalogSources()
        {
            var asmdefGuids = AssetDatabase.FindAssets("t:asmdef");
            foreach (var asmdefGuid in asmdefGuids)
            {
                var asmdefAssetPath = AssetDatabase.GUIDToAssetPath(asmdefGuid);
                if (string.IsNullOrEmpty(asmdefAssetPath))
                {
                    continue;
                }

                var catalogSource = GenerateConfigByAssembly(asmdefAssetPath);
                if (catalogSource != null)
                {
                    yield return catalogSource;
                }
            }
        }

        private static CatalogSource? GenerateConfigByAssembly(string asmdefAssetPath)
        {
            var asmdefDirectory = Path.GetDirectoryName(asmdefAssetPath);
            if (string.IsNullOrEmpty(asmdefDirectory))
            {
                return null;
            }

            var asmdefText = File.ReadAllText(asmdefAssetPath);
            var asmdefInfo = UnityEngine.JsonUtility.FromJson<AssemblyDefinitionFinder.AsmdefInfo>(asmdefText);

            // Only generate haikaraConfig.xml for assemblies that reference `Haikara.Runtime`.
            var hasHaikaraCoreReference = asmdefInfo.references.Any(x => x == "GUID:c48544e2906df9b4da907f99975a4093");
            if (!hasHaikaraCoreReference)
            {
                return null;
            }

            var searchInFolders = new[] { asmdefDirectory };
            // Collect all VisualTreeAssets within the target assembly.
            var catalogSource = new CatalogSource();

            var uxmlFileGuids = FindAssetsAndDependencyScriptFullPath(
                    filter: "glob:\"*.uxml\"",
                    searchInFolders: searchInFolders
                )
                .Select(x => x.assetGuid);

            catalogSource.UxmlFileGuids.AddRange(uxmlFileGuids);

            var ussFileGuids = FindAssetsAndDependencyScriptFullPath(
                    filter: "glob:\"*.uss\"",
                    searchInFolders: searchInFolders
                )
                .Select(x => x.assetGuid);
            
            catalogSource.UssFileGuids.AddRange(ussFileGuids);

            return catalogSource;
        }

        private static IEnumerable<(string scriptFileAssetPath, string assetGuid, string assetFullPath)>
            FindAssetsAndDependencyScriptFullPath(string filter, string[] searchInFolders)
        {
            var targetAssetGuids = AssetDatabase.FindAssets(filter, searchInFolders);
            foreach (var targetAssetGuid in targetAssetGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(targetAssetGuid);
                var assetFullPath = Path.GetFullPath(assetPath);
                var scriptFullPath = Path.ChangeExtension(assetFullPath, "cs");

                // Skip if the corresponding .cs file is not found in the same directory.
                if (!File.Exists(scriptFullPath))
                {
                    continue;
                }

                var scriptFilePath = System.IO.Path.ChangeExtension(assetPath, "cs");
                if (string.IsNullOrEmpty(scriptFilePath))
                {
                    continue;
                }

                yield return (scriptFilePath, targetAssetGuid, assetFullPath);
            }
        }
    }
}