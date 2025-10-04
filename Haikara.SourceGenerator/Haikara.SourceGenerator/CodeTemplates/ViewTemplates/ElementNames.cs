using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Haikara.Shared;

namespace SourceGenerator.CodeTemplates.ViewTemplates;

internal partial class ViewTemplate
{
    private static string ElementNamesCodeString(List<(XElement element, int index)> engineElements)
    {
        var codeString =
            $$"""
              
                      private struct ElementNames
                      {

              """;

        foreach (var elementNameInfoCodeString in GetElementNameInfoCodeString(engineElements))
        {
            codeString += elementNameInfoCodeString;
        }

        codeString +=
            $$"""
                      }
              """;

        codeString +=
            $$"""
              
                      private struct TemplateInfoList
                      {

              """;

        foreach (var elementNameInfoCodeString in GetTemplateInfoListCodeString(engineElements))
        {
            codeString += elementNameInfoCodeString;
        }

        codeString +=
            $$"""
                      }
              """;


        return codeString;
    }


    private static IEnumerable<string> GetElementNameInfoCodeString(
        List<(XElement element, int index)> engineElements
    )
    {
        foreach (var (fieldName, name) in ElementNameUtil.GetElementNameFields(engineElements))
        {
            yield return GenerateCode(fieldName: fieldName, name: name);
        }

        yield break;

        string GenerateCode(string fieldName, string name)
        {
            var codeString =
                $$"""
                              public const string {{fieldName}} = "{{name}}";

                  """;
            return codeString;
        }
    }

    private static IEnumerable<string> GetTemplateInfoListCodeString(
        List<(XElement element, int index)> engineElements
    )
    {
        foreach (var (fieldName, elementName, viewGuid, templateId)
                 in ElementNameUtil.GetTemplateInfoFields(engineElements))
        {
            yield return GenerateCode(
                fieldName: fieldName,
                elementName: elementName,
                viewGuid: viewGuid,
                templateId: templateId
            );
        }

        yield break;

        string GenerateCode(string fieldName, string elementName, string viewGuid, string templateId)
        {
            var codeString =
                $$"""
                              public static readonly TemplateInfo {{fieldName}} = new TemplateInfo(
                                  elementName: "{{elementName}}",
                                  viewGuid: "{{viewGuid}}",
                                  templateId: "{{templateId}}"
                              );

                  """;
            return codeString;
        }
    }
}