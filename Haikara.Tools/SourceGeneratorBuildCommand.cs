using System.Diagnostics;
using ConsoleAppFramework;

namespace Haikara.Tools;

/// <summary>
/// This command is intended to be called from "dotnet run" 
/// </summary>
public class SourceGeneratorBuildCommand
{
    private static readonly string SourceGeneratorProjectDir =
        Path.Combine("..", "Haikara.SourceGenerator", "Haikara.SourceGenerator");

    /// <summary>Build SourceGenerator and move the dll to Assets/Haikara/Plugins in UnityProject </summary>
    /// <param name="buildConfig">SourceGenerator build Configuration. Default is 'Debug'</param>
    [Command("build-sg")]
    public async Task<int> BuildSourceGenerator(string buildConfig = "Debug")
    {
        try
        {
            if (TryBuildSourceGenerator(buildConfig))
            {
                Console.WriteLine("");

                await CopyDll(buildConfig);

                Console.WriteLine("");

                Console.WriteLine("Command completed!");
                return 0;
            }
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"An error occurred: {ex.Message}");
        }

        return -1;
    }

    private bool TryBuildSourceGenerator(string buildConfig)
    {
        var sourceGeneratorProjectPath = Path.Combine(SourceGeneratorProjectDir, "Haikara.SourceGenerator.csproj");

        Console.WriteLine("--- Run Clean ---");
        var dotnetCleanProcessStartInfo = new ProcessStartInfo()
        {
            FileName = "dotnet",
            Arguments = "clean",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = SourceGeneratorProjectDir
        };
        using var cleanProcess = Process.Start(dotnetCleanProcessStartInfo);
        if (cleanProcess == null)
        {
            Console.WriteLine("Failed to start the process.");
            return false;
        }

        var cleanOutput = cleanProcess.StandardOutput.ReadToEnd();
        var cleanError = cleanProcess.StandardError.ReadToEnd();

        cleanProcess.WaitForExit();
        
        Console.WriteLine("--- Clean Output ---");
        Console.WriteLine(cleanOutput);
        
        if (cleanProcess.ExitCode == 0)
        {
            Console.WriteLine("Clean succeeded!");
        }
        else
        {
            Console.WriteLine("--- Build Error ---");
            Console.WriteLine(cleanError);
            Console.WriteLine($"Build failed with exit code: {cleanProcess.ExitCode}");

            return false;
        }
        

        var dotnetBuildProcessStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments =
                $"build -c {buildConfig} --force --no-incremental \"{sourceGeneratorProjectPath}\" ",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };

        Console.WriteLine("--- Run Build ---");
        Console.WriteLine($"{dotnetBuildProcessStartInfo.FileName} {dotnetBuildProcessStartInfo.Arguments}");

        using var process = Process.Start(dotnetBuildProcessStartInfo);
        if (process == null)
        {
            Console.WriteLine("Failed to start the process.");
            return false;
        }

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Console.WriteLine("--- Build Output ---");
        Console.WriteLine(output);

        if (process.ExitCode == 0)
        {
            Console.WriteLine("Build succeeded!");
            return true;
        }
        else
        {
            Console.WriteLine("--- Build Error ---");
            Console.WriteLine(error);
            Console.WriteLine($"Build failed with exit code: {process.ExitCode}");

            return false;
        }
    }

    private async Task CopyDll(string buildConfig)
    {
        var targetDirectory = Path.Combine("..", "Haikara", "Assets", "Haikara", "Plugins");
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        var buildResultDirectory = Path.Combine(
            SourceGeneratorProjectDir,
            "bin", buildConfig, "netstandard2.0"
        );

        const string sourceGeneratorDllFileName = "Haikara.SourceGenerator.dll";
        const string sharedDllFileName = "Haikara.Shared.dll";
        var jaResourceDllFilePath = Path.Combine("ja", "Haikara.SourceGenerator.resources.dll");
        var jaJpResourceDllFilePath = Path.Combine("ja-jp", "Haikara.SourceGenerator.resources.dll");

        var sourceGeneratorDllPath = Path.Combine(buildResultDirectory, sourceGeneratorDllFileName);
        var dstSourceGeneratorDllPath = Path.Combine(targetDirectory, sourceGeneratorDllFileName);

        var sourceSharedDllPath = Path.Combine(buildResultDirectory, sharedDllFileName);
        var dstSharedDllPath = Path.Combine(targetDirectory, sharedDllFileName);

        var sourceJaResourceDllFilePath = Path.Combine(buildResultDirectory, jaResourceDllFilePath);
        var dstJaResourceDllFilePath = Path.Combine(targetDirectory, jaResourceDllFilePath);

        var sourceJaJpResourceDllFilePath = Path.Combine(buildResultDirectory, jaJpResourceDllFilePath);
        var dstJaJpResourceDllFilePath = Path.Combine(targetDirectory, jaJpResourceDllFilePath);

        var tasks = new List<Task>();
        Console.WriteLine("--- Copy dll ---");

        // Copy Haikara.SourceGenerator.dll
        Console.WriteLine($"{Path.GetFullPath(sourceGeneratorDllPath)}\n" +
                          $"=>\n" +
                          $"{Path.GetFullPath(dstSourceGeneratorDllPath)}");
        tasks.Add(CopyFileAsync(sourceGeneratorDllPath, dstSourceGeneratorDllPath)); 

        // Copy Haikara.Shared.dll
        Console.WriteLine($"{Path.GetFullPath(sourceSharedDllPath)}\n" +
                          $"=>\n" +
                          $"{Path.GetFullPath(dstSharedDllPath)}");
        tasks.Add(CopyFileAsync(sourceSharedDllPath, dstSharedDllPath)); 

        // Copy ja/Haikara.SourceGenerator.resources.dll 
        Console.WriteLine($"{Path.GetFullPath(sourceJaResourceDllFilePath)}\n" +
                          $"=>\n" +
                          $"{Path.GetFullPath(dstJaResourceDllFilePath)}");
        tasks.Add(CopyFileAsync(sourceJaResourceDllFilePath, dstJaResourceDllFilePath)); 

        // Copy ja-jp/Haikara.SourceGenerator.resources.dll 
        Console.WriteLine($"{Path.GetFullPath(sourceJaJpResourceDllFilePath)}\n" +
                          $"=>\n" +
                          $"{Path.GetFullPath(dstJaJpResourceDllFilePath)}");
        tasks.Add(CopyFileAsync(sourceJaJpResourceDllFilePath, dstJaJpResourceDllFilePath));

        await Task.WhenAll(tasks);

        Console.WriteLine("Copy dll succeeded!");
    }

    private async Task CopyFileAsync(string sourcePath, string destPath)
    {
        using (FileStream sourceStream = File.OpenRead(sourcePath))
        {
            using (FileStream destinationStream = File.Create(destPath))
            {
                await sourceStream.CopyToAsync(destinationStream);
            }
        }
    }
}