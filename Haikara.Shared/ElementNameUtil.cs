using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Haikara.Shared
{
    public static class ElementNameUtil
    {
        public static IEnumerable<(string fieldName, string elementName, string viewGuid, string templateId)>
            GetTemplateInfoFields(List<(XElement element, int index)> engineElements)
        {
            var templateIdToViewGuid = new Dictionary<string, string>();
            foreach (var (element, index) in engineElements.Where(x => x.element.Name.LocalName == "Template"))
            {
                var templateId = element.Attribute("name");
                if (templateId == null)
                {
                    continue;
                }

                var srcAttribute = element.Attribute("src");
                if (srcAttribute == null)
                {
                    continue;
                }

                // Get the GUID from the source attribute.
                var srcValue = srcAttribute.Value;
                if (string.IsNullOrEmpty(srcValue))
                {
                    continue;
                }

                var guid = "";
                var match = Regex.Match(srcValue, @"[\?&]guid=([^&#]+)");
                if (match.Success && match.Groups.Count > 1)
                {
                    guid = match.Groups[1].Value;
                }

                if (string.IsNullOrEmpty(guid))
                {
                    continue;
                }

                templateIdToViewGuid.Add(templateId.Value, guid);
            }

            foreach (var (templateInstanceElement, index) in engineElements.Where(
                         x => x.element.Name.LocalName == "Instance")
                    )
            {
                var templateAttribute = templateInstanceElement.Attribute("template");
                if (templateAttribute == null)
                {
                    continue;
                }

                var nameAttribute = templateInstanceElement.Attribute("name");
                if (nameAttribute == null)
                {
                    continue;
                }

                var elementName = nameAttribute.Value;
                if (string.IsNullOrWhiteSpace(elementName))
                {
                    continue;
                }

                var templateId = templateAttribute.Value;
                if (string.IsNullOrWhiteSpace(templateId))
                {
                    continue;
                }

                if (!templateIdToViewGuid.TryGetValue(templateId, out var viewGuid))
                {
                    continue;
                }

                yield return (
                    fieldName: FormatElementNameToCSharpCode(elementName),
                    elementName: elementName,
                    viewGuid: viewGuid,
                    templateId: templateId
                );
            }
        }


        public static IEnumerable<(string fieldName, string name)> GetElementNameFields(
            List<(XElement element, int index)> engineElements
        )
        {
            var elementTypeGroups = engineElements
                .Where(x => x.element.Name.LocalName != "Template")
                .GroupBy(x => x.element.Name.LocalName);

            foreach (var elementTypeGroup in elementTypeGroups)
            {
                var elementType = elementTypeGroup.Key;
                if (string.IsNullOrEmpty(elementType))
                {
                    continue;
                }

                // Get the value of 'name' and convert it into a valid C# identifier.
                var elementNames = elementTypeGroup
                    .Select(x => x.element.Attribute("name"))
                    .Select(x => x?.Value)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct()
                    .Select(x => (fieldName: FormatElementNameToCSharpCode(x), name: x));

                foreach (var (fieldName, name) in elementNames)
                {
                    yield return (fieldName, name);
                }
            }
        }

        public static string FormatElementNameToCSharpCode(string name)
        {
            var result = name;
            string[] words = Regex.Split(name, @"[-_]+")
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();

            if (words.Length != 0)
            {
                result = "";
                foreach (var word in words)
                {
                    var convertedToUpper = false;

                    foreach (var wordChar in word)
                    {
                        if (char.IsDigit(wordChar))
                        {
                            continue;
                        }

                        if (!convertedToUpper)
                        {
                            result += char.ToUpperInvariant(wordChar);
                            convertedToUpper = true;
                        }
                        else
                        {
                            result += wordChar;
                        }
                    }
                }
            }

            return result;
        }
    }
}