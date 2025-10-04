using System.IO;
using Haikara.Shared;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.StyleTemplates;

internal class StyleTemplate(StyleFileInfo styleFileInfo) : CodeTemplate<GenerationSource>
{
    public override string BuildCodeString(GenerationSource generationSource)
    {
        var namespaceString = $"{generationSource.GetTypeSymbol().ContainingNamespace}";
        if (string.IsNullOrWhiteSpace(namespaceString))
        {
            namespaceString = "Haikara.Generated";
        }

        var className = generationSource.GetTypeSymbol().Name;

        var codeString = $$"""
                           using System.Collections.Generic;
                           using {{DefinedStrings.CatalogNamespace}};
                           using UnityEngine.UIElements;
                           using System.Threading.Tasks;

                           namespace {{namespaceString}}
                           {
                               #nullable enable
                               public partial class {{className}}
                               {
                                   public const string UssGuid = "{{generationSource.AssetGuid}}";
                                   public {{generationSource.ReturnType("string")}} GetGuid()
                                   {
                                       return UssGuid;
                                   }
                                   
                                   public static async Task<StyleSheet?> GetStyleSheet()
                                   {
                                       return await RuntimeUICatalog.Instance.LoadStyleSheetAsync(UssGuid);
                                   }
                                   
                                   public {{generationSource.ReturnType(DefinedStrings.AssetReferenceMode)}} AssetReferenceMode => {{generationSource.ReferenceModeString()}};
                                   
                           """;

        codeString +=
            $$"""
              
                      public class UsedClassNames
                      {

              """;
        foreach (var ussClassName in styleFileInfo.UsedClassNames)
        {
            var fieldName = StringUtil.GetValidCSharpString(ussClassName).FormatToCSharpCodeConventions();
            if (string.IsNullOrEmpty(fieldName))
            {
                continue;
            }
            
            codeString +=
                $$"""
                  
                              public const string {{fieldName}} = "{{ussClassName}}";
                              
                  """;
        }

        codeString +=
            $$"""
              
                      }
                          
              """;


        codeString +=
            $$"""
              
                  }
                  #nullable restore
              }
              """;

        return codeString;
    }
}