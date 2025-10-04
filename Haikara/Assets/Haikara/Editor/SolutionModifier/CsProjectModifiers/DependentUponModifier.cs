using System.IO;
using System.Xml.Linq;

namespace Haikara.Editor.SolutionModifier.CsProjectModifiers
{
    /// <summary>
    /// Sets the corresponding .uxml file (same name, same directory) as the "DependentUpon" target for a .cs file.
    /// This is triggered right after the .csproj file is generated, so it executes before the SourceGenerator runs.
    /// </summary>
    internal class DependentUponModifier : ICsProjectModifier
    {
        public void Modify(XDocument doc, string csProjectPath, out ModifyResult modifyResult)
        {
            var xNamespace = doc.Root?.GetDefaultNamespace();
            if (xNamespace == null)
            {
                xNamespace = "";
            }

            modifyResult = ModifyResult.DidNotModified;
            foreach (var compileElement in doc.Descendants(xNamespace + "Compile"))
            {
                var includeAttribute = compileElement.Attribute("Include");
                if (includeAttribute == null)
                {
                    continue;
                }

                var compileFileAssetPath = includeAttribute.Value;
                if (!File.Exists(compileFileAssetPath))
                {
                    continue;
                }

                if (TrySetDependentUpon(dependentFileExtension: ".uxml"))
                {
                    modifyResult = ModifyResult.Modified;
                    continue;
                }

                if (TrySetDependentUpon(dependentFileExtension: ".uss"))
                {
                    modifyResult = ModifyResult.Modified;
                    continue;
                }

                continue;

                bool TrySetDependentUpon(string dependentFileExtension)
                {
                    var assetFilePath = Path.ChangeExtension(compileFileAssetPath, dependentFileExtension);

                    if (!File.Exists(assetFilePath))
                    {
                        return false;
                    }

                    var assetFileName = Path.GetFileName(assetFilePath);
                    compileElement.Add(new XElement(xNamespace + "DependentUpon", assetFileName));
                    return true;
                }
            }
        }
    }
}