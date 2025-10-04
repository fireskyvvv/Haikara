using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.ViewTemplates;

internal partial class ViewTemplate
{
    private static string PartialViewFrame(GenerationSource generationSource)
    {
        var typeSymbol = generationSource.UiContext.TargetSymbol;
        
        var namespaceString = $"{typeSymbol.ContainingNamespace}";
        if (string.IsNullOrWhiteSpace(namespaceString))
        {
            namespaceString = "Haikara.Generated";
        }

        var className = typeSymbol.Name;
        var elementPropertyFieldList = GetElementPropertyFieldInfoList(generationSource);
        var templatePropertyFieldList = GetTemplatePropertyFieldInfoList(generationSource);

        var uxmlFilePath = Path.ChangeExtension(generationSource.CsFilePath, "uxml");
        var doc = XDocument.Load(uxmlFilePath);
        var engineElements = doc.Descendants()
            .Where(e => e.Name.Namespace == DefinedStrings.UiElementsNamespace)
            .Select((x, index) => (element: x, index: index))
            .ToList();


        var codeString = $$"""
                           using Haikara.Runtime.Bindable;
                           using Haikara.Runtime.View;
                           using UnityEngine;
                           using Unity.Properties;
                           using UnityEngine.UIElements;

                           namespace {{namespaceString}}
                           {
                               #nullable enable
                               {{SyntaxFacts.GetText(typeSymbol.DeclaredAccessibility)}} partial class {{className}}
                               {
                                   public const string UxmlGuid = "{{generationSource.AssetGuid}}";
                                   public {{generationSource.ReturnType("string")}} GetGuid()
                                   {
                                       return UxmlGuid;
                                   }
                                   public {{generationSource.ReturnType(DefinedStrings.AssetReferenceMode)}} AssetReferenceMode => {{generationSource.ReferenceModeString()}};
                                   {{InitializeComponentCodeString(generationSource, elementPropertyFieldList, templatePropertyFieldList, engineElements)}}
                                   {{ElementNamesCodeString(engineElements)}}
                               }
                               #nullable restore
                           }
                           """;

        return codeString;
    }
}