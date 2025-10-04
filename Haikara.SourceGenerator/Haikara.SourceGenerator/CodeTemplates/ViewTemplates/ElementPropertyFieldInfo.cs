using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.ViewTemplates;

internal partial class ViewTemplate
{
    private class ElementPropertyFieldInfo
    {
        public enum ElementPropertyType
        {
            BindableProperty,
            ManipulatorProperty,
            Invalid
        }

        public IFieldSymbol FieldSymbol { get; }
        public INamedTypeSymbol NamedTypeSymbol { get; }
        public ElementPropertyType Type { get; }
        public string DisplayString { get; set; } = "";

        public ElementPropertyFieldInfo(IFieldSymbol fieldSymbol, INamedTypeSymbol namedTypeSymbol,
            ElementPropertyType type)
        {
            FieldSymbol = fieldSymbol;
            NamedTypeSymbol = namedTypeSymbol;
            Type = type;
        }
    }

    private static List<ElementPropertyFieldInfo> GetElementPropertyFieldInfoList(
        GenerationSource generationSource
    )
    {
        return generationSource.GetTypeSymbol()
            .GetMembers()
            .OfType<IFieldSymbol>()
            .Select(fieldSymbol =>
            {
                if (fieldSymbol.Type is not INamedTypeSymbol namedTypeSymbol)
                {
                    return null;
                }

                var displayString = namedTypeSymbol.OriginalDefinition.ToDisplayString();

                if (namedTypeSymbol.HasInterfaceTypeName(DefinedStrings.BindablePropertyInterfaceName))
                {
                    return new ElementPropertyFieldInfo(
                        fieldSymbol,
                        namedTypeSymbol,
                        ElementPropertyFieldInfo.ElementPropertyType.BindableProperty);                    
                }

                if (displayString == DefinedStrings.ManipulatorElementPropertyDisplayString)
                {
                    return new ElementPropertyFieldInfo(
                        fieldSymbol,
                        namedTypeSymbol,
                        ElementPropertyFieldInfo.ElementPropertyType.ManipulatorProperty);
                }

                return null;
            })
            .Where(x => x != null)
            .OfType<ElementPropertyFieldInfo>()
            .ToList();
    }
}