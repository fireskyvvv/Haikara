using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceGenerator.Tests;

public partial class GeneratorTest
{
    // Test files
    // The actual files are located under "HaikaraDev/Tests/" in the Unity project.
    private static string TestViewDirectoryName => Path.Combine(TestAssetsRootFolderName, "View");
    private const string TestViewFileName = "TestView.cs";
    private const string TestSubViewFileName = "TestSubView.cs";
    private static string TestViewModelDirectoryName => Path.Combine(TestAssetsRootFolderName, "ViewModel");
    private const string TestViewModelFileName = "TestViewModel.cs";

    [Fact]
    public Task RunViewGeneratorResults()
    {
        var driver = BuildViewGeneratorDriver();
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

    private GeneratorDriver BuildViewGeneratorDriver()
    {
        var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        var viewCsFilePath = Path.Combine(executionPath, TestViewDirectoryName, $"{TestViewFileName}");
        var subViewCsFilePath = Path.Combine(executionPath, TestViewDirectoryName, $"{TestSubViewFileName}");

        var viewModelCsFilePath = Path.Combine(executionPath, TestViewModelDirectoryName, $"{TestViewModelFileName}");

        var viewSource = File.ReadAllText(viewCsFilePath);
        var subViewSource = File.ReadAllText(subViewCsFilePath);

        var viewModelSource = File.ReadAllText(viewModelCsFilePath);

        var testViewSyntaxTree = CSharpSyntaxTree.ParseText(viewSource, path: viewCsFilePath);
        var testSubViewSyntaxTree = CSharpSyntaxTree.ParseText(subViewSource, path: subViewCsFilePath);
        var testViewModelSyntaxTree = CSharpSyntaxTree.ParseText(viewModelSource, path: viewModelCsFilePath);

        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        var references = GetAllReferences(executionPath).ToList();

        var compilation = CSharpCompilation.Create(
            "Haikara.Tests",
            new[]
            {
                testViewSyntaxTree,
                testSubViewSyntaxTree,
                testViewModelSyntaxTree,
            },
            references,
            compilationOptions
        );

        var viewGenerator = new ViewGenerator();
        var driver = CSharpGeneratorDriver.Create(
            generators: new[] { viewGenerator.AsSourceGenerator() }
        );

        return driver.RunGenerators(compilation);
    }
}