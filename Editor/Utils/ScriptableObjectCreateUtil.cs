using System.IO;
using UnityEditor;
using UnityEngine;

namespace Haikara.Editor.Utils
{
    internal static class ScriptableObjectCreateUtil
    {
        public static T Create<T>(string createFolderPath = "", string assetName = "", bool focusAfterCreated = true)
            where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(createFolderPath))
            {
                createFolderPath = GetSelectedFolderPath();
            }

            var newInstance = ScriptableObject.CreateInstance<T>();
            if (string.IsNullOrEmpty(assetName))
            {
                assetName = typeof(T).Name;
            }

            var newSettingsAssetPath = $"{createFolderPath}/{assetName}.asset";

            AssetDatabase.CreateAsset(newInstance, newSettingsAssetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (focusAfterCreated)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = newInstance;
            }

            return newInstance;
        }


        private static string GetSelectedFolderPath()
        {
            var path = "Assets";
            var activeObject = Selection.activeObject;
            if (activeObject == null)
            {
                // If nothing is selected, use the 'Assets' folder as the target.
                return path;
            }

            path = AssetDatabase.GetAssetPath(activeObject);

            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }
            }

            return path;
        }
    }
}