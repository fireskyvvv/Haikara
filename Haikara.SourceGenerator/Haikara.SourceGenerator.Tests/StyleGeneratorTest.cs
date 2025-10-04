using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceGenerator.Tests;

public partial class GeneratorTest
{
    // Test files
    // The actual files are located under "HaikaraDev/Tests/" in the Unity project.
    private static string TestStyleDirectoryName => Path.Combine(TestAssetsRootFolderName, "Style");
    private const string TestStyleFileName = "TestStyle.cs";

    [Fact]
    public Task RunStyleGeneratorResults()
    {
        var driver = BuildStyleGeneratorDriver();
        var generatedSources = driver.GetRunResult().Results
            .SelectMany(x => x.GeneratedSources)
            .ToList();

        Assert.NotEmpty(generatedSources);

        var verifyTasks = new List<Task>();
        foreach (var generatedSource in generatedSources)
        {
            var verifySettings = new VerifySettings();
            verifySettings.UseDirectory("snapshots");
            verifySettings.UseFileName(generatedSource.HintName);

            var task = Verify(generatedSource.SourceText.ToString(), extension: "txt", verifySettings);
            verifyTasks.Add(task);
        }

        return Task.WhenAll(verifyTasks);
    }

    private GeneratorDriver BuildStyleGeneratorDriver()
    {
        var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        var styleCsFilePath = Path.Combine(executionPath, TestStyleDirectoryName, $"{TestStyleFileName}");

        var styleSource = File.ReadAllText(styleCsFilePath);

        var testStyleSyntaxTree = CSharpSyntaxTree.ParseText(styleSource, path: styleCsFilePath);

        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        var references = GetAllReferences(executionPath);

        var compilation = CSharpCompilation.Create(
            "Haikara.Tests",
            new[]
            {
                testStyleSyntaxTree,
            },
            references,
            compilationOptions
        );

        var styleGenerator = new StyleGenerator();
        var driver = CSharpGeneratorDriver.Create(
            generators: new[] { styleGenerator.AsSourceGenerator() }
        );

        return driver.RunGenerators(compilation);
    }
}