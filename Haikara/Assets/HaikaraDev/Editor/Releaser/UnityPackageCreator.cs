using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HaikaraDev.Editor.Releaser
{
    public static class UnityPackageCreator
    {
        [MenuItem("Haikara/Releaser/Create .unitypackage")]
        private static void CreateUnityPackage()
        {
            var exportDirectory = EditorUtility.OpenFolderPanel("Select Folder", "", "");
            if (string.IsNullOrEmpty(exportDirectory))
            {
                return;
            }

            CreateUnityPackage(exportDirectory);
        }

        private static void CI_CreateUnityPackage()
        {
            var tag = GetArgument("-tag");
            if (string.IsNullOrEmpty(tag))
            {
                // 引数が渡されなかった場合のエラー処理
                throw new ArgumentException("'-tag' command line argument not provided. Cannot create package without a version.");
            }

            CreateUnityPackage("build", tag: tag);
        }

        private static void CreateUnityPackage(string exportDirectory, string tag = null)
        {
            const string assetPath = "Assets/Haikara";
            var packageName = "Haikara.unitypackage";
            if (!string.IsNullOrEmpty(tag))
            {
                packageName = $"Haikara-v{tag}.unitypackage";
            }
            
            if (!Directory.Exists(exportDirectory))
            {
                Directory.CreateDirectory(exportDirectory);
            }

            var exportPath = Path.Combine(exportDirectory, packageName);

            Debug.Log($"Exporting package to: {exportPath}");

            AssetDatabase.ExportPackage(
                assetPath,
                exportPath,
                ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies
            );

            Debug.Log("Package export complete.");
        }
        
        private static string GetArgument(string name)
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }
            return null;
        }
    }
}