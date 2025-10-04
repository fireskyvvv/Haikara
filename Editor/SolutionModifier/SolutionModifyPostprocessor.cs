using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Haikara.Editor.SolutionModifier.CsProjectModifiers;
using Haikara.Runtime.Util;
using UnityEditor;

namespace Haikara.Editor.SolutionModifier
{
    internal class SolutionModifyPostprocessor : AssetPostprocessor
    {
        private static string OnGeneratedSlnSolution(string path, string content)
        {
            return content;
        }

        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            /*var uiAssetFiles = importedAssets
                .Concat(deletedAssets)
                .Concat(movedAssets)
                .Concat(movedFromAssetPaths)
                .Where(x =>
                {
                    var extension = Path.GetExtension(x);
                    return extension is ".uxml" or ".uss";
                });*/
        }

        private static string OnGeneratedCSProject(string path, string content)
        {
            XDocument doc;
            try
            {
                doc = XDocument.Parse(content);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(
                    HaikaraLogUtil.GetMessage($"XDocument Parse Error. Path:{path}\n" + $"{e}")
                );

                return content;
            }

            var modifiers = new List<ICsProjectModifier>()
            {
                new DependentUponModifier()
            };

            var isModified = false;
            foreach (var modifier in modifiers)
            {
                modifier.Modify(doc: doc, csProjectPath: path, out var modifyResult);

                if (modifyResult == ModifyResult.Interrupted)
                {
                    isModified = false;
                    break;
                }

                switch (modifyResult)
                {
                    case ModifyResult.Modified:
                        isModified = true;
                        break;
                    case ModifyResult.DidNotModified:
                        break;
                }
            }

            if (!isModified)
            {
                return content;
            }

            UnityEngine.Debug.Log(HaikaraLogUtil.GetMessage($"CsProjectModified. {path}"));
            return doc.ToString();
        }
    }
}