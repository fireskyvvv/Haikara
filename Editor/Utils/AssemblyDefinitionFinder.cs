using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Haikara.Editor.Utils
{
    public static class AssemblyDefinitionFinder
    {
        private static readonly Dictionary<string, AsmdefInfo> AsmdefAssetPathToInfoCache = new();

        [Serializable]
        internal class AsmdefInfo
        {
            public string name = string.Empty;
            public string[] references = Array.Empty<string>();
        }

        /// <summary>
        /// Gets the AssetPath of the Assembly Definition (.asmdef) file that a given asset GUID belongs to.
        /// </summary>
        /// <remarks>
        /// For example, given the following directory structure:
        /// <code>
        /// Assets
        ///   └ Hai
        ///     ├ Kara
        ///     │    └ Script.cs
        ///     └ Hai.asmdef
        /// </code>
        /// Providing the GUID for "Script.cs" will return "Assets/Hai/Hai.asmdef".
        /// </remarks>
        public static bool TryGetBelongingAssemblyDefinitionAssetPath(string assetPath, out string asmdefAssetPath)
        {
            asmdefAssetPath = "";
            var currentDirectory = Path.GetDirectoryName(assetPath);
            while (!string.IsNullOrEmpty(currentDirectory))
            {
                var asmdefGuids = AssetDatabase.FindAssets("t:asmdef", new[] { currentDirectory });
                if (asmdefGuids.Length > 0)
                {
                    asmdefAssetPath = AssetDatabase.GUIDToAssetPath(asmdefGuids[0]);
                    if (!string.IsNullOrEmpty(asmdefAssetPath))
                    {
                        return true;
                    }
                }

                currentDirectory = Path.GetDirectoryName(currentDirectory);
            }

            return false;
        }

        internal static AsmdefInfo GetAsmdefInfo(string asmdefAssetPath)
        {
            if (!AsmdefAssetPathToInfoCache.TryGetValue(asmdefAssetPath, out var cache))
            {
                var asmdefAsset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(asmdefAssetPath);
                var asmdefInfo = JsonUtility.FromJson<AsmdefInfo>(asmdefAsset.text);
                cache = asmdefInfo;
                AsmdefAssetPathToInfoCache.Add(asmdefAssetPath, cache);
            }

            return cache;
        }
    }
}