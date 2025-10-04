using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using DiffEngine;

namespace SourceGenerator.Tests;

public static class Initializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        DiffTools.UseOrder(DiffTool.VisualStudioCode);
        
        var dateTimeRegex = new Regex(@"^// Generated: .*$", RegexOptions.Multiline);
        
        var frameworkRegex = new Regex(@"^//     Runtime FrameworkDescription:.*$", RegexOptions.Multiline);

        VerifierSettings.AddScrubber(builder =>
        {
            var content = builder.ToString();
            content = dateTimeRegex.Replace(content, "// Generated: [DATE_TIME]");
            content = frameworkRegex.Replace(content, "//     Runtime FrameworkDescription: [FRAMEWORK]");
            
            builder.Clear();
            builder.Append(content);
        });
    }
}