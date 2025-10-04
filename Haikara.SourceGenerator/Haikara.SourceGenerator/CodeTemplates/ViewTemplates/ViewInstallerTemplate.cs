using System.Collections.Generic;
using System.Collections.Immutable;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.ViewTemplates;

internal class ViewInstaller : CodeTemplate<IEnumerable<GenerationSource>>
{
    private readonly string _assemblyName;

    public ViewInstaller(string assemblyName)
    {
        _assemblyName = assemblyName;
    }

    public override string BuildCodeString(IEnumerable<GenerationSource> generationSource)
    {
        return Build(generationSource, _assemblyName);
    }

    private static string Build(IEnumerable<GenerationSource> executeSources, string assemblyName)
    {
        var namespaceString = $"{StringUtil.GetValidCSharpString(assemblyName, allowPeriod: true)}";
        if (string.IsNullOrWhiteSpace(namespaceString))
        {
            namespaceString = "Haikara.Generated";
        }

        var codeString = "";
        codeString +=
            $$"""
              namespace {{namespaceString}}
              {
                  #nullable enable
                  public partial class ViewInstaller : {{DefinedStrings.ViewInstallerBaseTypeName}}<ViewInstaller>
                  {
                      protected override void InstallInternal()
                      {
                      
              """;

        foreach (var executeSource in executeSources)
        {
            var id = executeSource.AssetGuid;
            var fullType = executeSource.UiContext.TargetSymbol.ToDisplayString();
            codeString +=
                $$"""
                  
                              {{DefinedStrings.ViewProviderTypeName}}.Instance.Register("{{id}}", () => new {{fullType}}());
                              
                  """;
        }

        codeString +=
            $$"""
              
                      }
                  }
                  #nullable restore
              }
              """;

        return codeString;
    }
}