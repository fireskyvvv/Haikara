using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.ViewTemplates;

internal partial class ViewTemplate
{
    private class TemplatePropertyFieldInfo
    {
        public IFieldSymbol FieldSymbol { get; }
        public INamedTypeSymbol NamedTypeSymbol { get; }

        public TemplatePropertyFieldInfo(IFieldSymbol fieldSymbol, INamedTypeSymbol namedTypeSymbol)
        {
            FieldSymbol = fieldSymbol;
            NamedTypeSymbol = namedTypeSymbol;
        }
    }

    private static List<TemplatePropertyFieldInfo> GetTemplatePropertyFieldInfoList(
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

                if (namedTypeSymbol.HasInterfaceTypeName(DefinedStrings.TemplatePropertyInterfaceName))
                {
                    return new TemplatePropertyFieldInfo(
                        fieldSymbol,
                        namedTypeSymbol
                    );
                }

                return null;
            })
            .Where(x => x != null)
            .OfType<TemplatePropertyFieldInfo>()
            .ToList();
    }
}