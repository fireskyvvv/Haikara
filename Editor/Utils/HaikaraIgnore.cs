using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Haikara.Editor.Utils
{
    public static class HaikaraIgnore
    {
        private const string IgnoreFileName = ".haikaraIgnore";
        private static string HaikaraIgnorePath => Path.Combine(Application.dataPath, IgnoreFileName);

        private static string IgnoreGuidListCacheFilePath =>
            Path.Combine(Application.dataPath, "..", "Library", $"{IgnoreFileName}.cache");

        public static List<string> GetUIToolKitFileGuids()
        {
            var ignoreUIToolKitFileGuids = new List<string>();
            if (!File.Exists(HaikaraIgnorePath))
            {
                if (File.Exists(IgnoreGuidListCacheFilePath))
                {
                    File.Delete(IgnoreGuidListCacheFilePath);
                }

                return ignoreUIToolKitFileGuids;
            }

            var ignoreGuids = new List<string>();
            foreach (var line in ReadIgnoreFileLines())
            {
                if (line.StartsWith("!"))
                {
                    RemoveFromIgnore(line[1..], ignoreGuids);
                }
                else
                {
                    AddToIgnore(line, ignoreGuids);
                }
            }

            var allUxmlAndUssGuids = AssetDatabase.FindAssets("glob:\"*.uss\" glob:\"*.uxml\"");
            ignoreUIToolKitFileGuids.AddRange(allUxmlAndUssGuids.Where(x => ignoreGuids.Contains(x)));

            return ignoreUIToolKitFileGuids;
        }

        private static IEnumerable<string> ReadIgnoreFileLines()
        {
            foreach (var line in File.ReadAllLines(HaikaraIgnorePath))
            {
                yield return line;
            }
        }

        private static void AddToIgnore(string line, List<string> ignoreGuids)
        {
            // File
            if (line.EndsWith(".cs") || line.EndsWith(".uss"))
            {
                var ignoreFileGuids = AssetDatabase.FindAssets($"glob:\"{line}\"");
                ignoreGuids.AddRange(ignoreFileGuids);
            }
            else
            {
                // Directory
                var directoryLine = line;
                if (!line.EndsWith("/"))
                {
                    directoryLine += "/";
                }

                var ussPattern = directoryLine + "**.uss";
                var ignoreUssGuids = AssetDatabase.FindAssets($"glob:\"{ussPattern}\"");
                ignoreGuids.AddRange(ignoreUssGuids);

                var uxmlPattern = directoryLine + "**.uxml";
                var ignoreUxmlGuids = AssetDatabase.FindAssets($"glob:\"{uxmlPattern}\"");
                ignoreGuids.AddRange(ignoreUxmlGuids);
            }
        }

        private static void RemoveFromIgnore(string line, List<string> ignoreGuids)
        {
            // File
            if (line.EndsWith(".cs") || line.EndsWith(".uss"))
            {
                var removeFileGuids = AssetDatabase.FindAssets($"glob:\"{line}\"");
                Remove(removeFileGuids);
            }
            else
            {
                // Directory
                var directoryLine = line;
                if (!line.EndsWith("/"))
                {
                    directoryLine += "/";
                }

                var ussPattern = directoryLine + "**.uss";
                var removeUssGuids = AssetDatabase.FindAssets($"glob:\"{ussPattern}\"");
                Remove(removeUssGuids);

                var uxmlPattern = directoryLine + "**.uxml";
                var removeUxmlGuids = AssetDatabase.FindAssets($"glob:\"{uxmlPattern}\"");
                Remove(removeUxmlGuids);
            }

            return;

            void Remove(string[] removeGuids)
            {
                foreach (var removeGuid in removeGuids)
                {
                    if (ignoreGuids.Contains(removeGuid))
                    {
                        ignoreGuids.Remove(removeGuid);
                    }
                }
            }
        }
    }
}