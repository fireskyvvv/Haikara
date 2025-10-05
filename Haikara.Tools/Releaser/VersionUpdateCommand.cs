using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using ConsoleAppFramework;

namespace Haikara.Tools.Releaser;

public partial class VersionUpdateCommand
{
    [GeneratedRegex("^[0-9]+\\.[0-9]+\\.[0-9]+$")]
    private static partial Regex SemVerRegex();

    [Command("release-update-version")]
    public void UpdateVersion(string version, string packageJsonPath)
    {
        Console.WriteLine($"Starting update version process for v{version}");
        ValidateVersionFormat(version);
        UpdatePackageJson(version, packageJsonPath);
    }

    private void ValidateVersionFormat(string version)
    {
        Console.WriteLine("Validating version format...");
        if (!SemVerRegex().IsMatch(version))
        {
            var errorMessage =
                $"Error: Version format is invalid. Received: '{version}'. Please use a format like '1.2.3'.";
            Console.WriteLine(errorMessage);
            throw new ArgumentException(errorMessage);
        }
    }

    private void UpdatePackageJson(string version, string packageJsonPath)
    {
        Console.WriteLine($"Updating '{packageJsonPath}' to version {version}...");
        if (!File.Exists(packageJsonPath))
        {
            throw new FileNotFoundException($"package.json not found at: {packageJsonPath}");
        }

        var jsonString = File.ReadAllText(packageJsonPath);
        var jsonNode = JsonNode.Parse(jsonString)!;
        jsonNode["version"] = version;

        // 整形して書き込み
        File.WriteAllText(packageJsonPath, jsonNode.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }
}