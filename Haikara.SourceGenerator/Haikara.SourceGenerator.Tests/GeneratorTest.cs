using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit.Abstractions;

namespace SourceGenerator.Tests;

public partial class GeneratorTest(ITestOutputHelper testOutputHelper)
{
    private const string TestAssetsRootFolderName = "TestAssets";

    private IEnumerable<MetadataReference> GetAllReferences(string executionPath)
    {
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var references = new List<MetadataReference>(
            loadedAssemblies
                .Where(assembly => !assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location))
                .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
        );

        // Reference to the proxy project.
        // The .csproj file is named 'Haikara.Runtime.ForTests', but the output assembly is named 'Haikara.Runtime'.
        // This is because the SourceGenerator filters by the assembly name ('Haikara.Runtime') to check for references.
        var proxyAssemblyPath = Path.Combine(executionPath, "Haikara.Runtime.dll");
        if (!File.Exists(proxyAssemblyPath))
        {
            throw new FileNotFoundException(
                $"Proxy assembly not found: {proxyAssemblyPath}. Make sure Haikara.Runtime.ForTests project is referenced and built.");
        }

        if (references.All(r => Path.GetFileName(r.Display) != "Haikara.Runtime.dll"))
        {
            references.Add(MetadataReference.CreateFromFile(proxyAssemblyPath));
        }

        var unityEditorPath = Environment.GetEnvironmentVariable("UNITY_EDITOR_PATH");
        if (string.IsNullOrEmpty(unityEditorPath))
        {
            testOutputHelper.WriteLine(
                "UNITY_EDITOR_PATH environment variable is not set. Unity assemblies might not be found.");
        }
        else
        {
            var managedPath = Path.Combine(unityEditorPath, "Data", "Managed", "UnityEngine");
            var unityDlls = new[]
            {
                "UnityEngine.CoreModule.dll",
                "UnityEngine.UIElementsModule.dll",
                "UnityEngine.PropertiesModule.dll"
            };

            foreach (var dll in unityDlls)
            {
                var dllPath = Path.Combine(managedPath, dll);
                if (!File.Exists(dllPath))
                {
                    dllPath = Path.Combine(Path.GetDirectoryName(managedPath)!, dll);
                }

                if (File.Exists(dllPath))
                {
                    if (references.All(r => Path.GetFileName(r.Display) != dll))
                    {
                        references.Add(MetadataReference.CreateFromFile(dllPath));
                    }
                }
                else
                {
                    testOutputHelper.WriteLine($"Warning: Unity DLL not found at {dllPath}");
                }
            }
        }

        return references;
    }
}