using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.ViewTemplates;

internal partial class ViewTemplate
{
    private static string InitializeComponentCodeString(
        GenerationSource generationSource,
        List<ElementPropertyFieldInfo> elementPropertyFieldInfoList,
        List<TemplatePropertyFieldInfo> templatePropertyFieldInfoList,
        List<(XElement element, int index)> engineElements
    )
    {
        var codeString =
            $$"""
              
                      protected {{generationSource.ReturnType("void")}} InitializeComponentInternal()
                      {
                      
              """;

        foreach (var initializeCodeString in
                 GetComponentInitializeCodeString(elementPropertyFieldInfoList, templatePropertyFieldInfoList))
        {
            codeString +=
                $$"""
                  
                              {{initializeCodeString}}

                  """;
        }

        foreach (var clickCommandInfo in GetClickCommandInfo(generationSource))
        {
            codeString +=
                $$"""
                  
                              ElementProperties.Add(
                                  ManipulatorProperty<VisualElement>.Create(
                                      elementNameInfo: new ElementNameInfo(
                                          elementName: {{clickCommandInfo.elementName}},
                                          index: {{clickCommandInfo.indexString}},
                                          findType: {{clickCommandInfo.findTypeString}}
                                      ),
                                      manipulator: new Clickable((evt) => { {{clickCommandInfo.methodName}}(evt); })
                                  )
                              );
                  """;
        }

        codeString +=
            $$"""
              
              
                          ElementProperties.SortElementPropertiesByPriority();
              """;
        codeString +=
            $$"""
              
                      }
              """;

        return codeString;
    }

    private static IEnumerable<string> GetComponentInitializeCodeString(
        List<ElementPropertyFieldInfo> elementPropertyFields,
        List<TemplatePropertyFieldInfo> templatePropertyFields)
    {
        foreach (var templatePropertyField in templatePropertyFields)
        {
            yield return $"TemplateProperties.Add({templatePropertyField.FieldSymbol});";
        }

        foreach (var elementPropertyField in elementPropertyFields)
        {
            yield return $"ElementProperties.Add({elementPropertyField.FieldSymbol});";
        }
    }
}