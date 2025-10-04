using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Utils;

namespace SourceGenerator.Providers;

public static class HaikaraUIProvider
{
    public static IncrementalValuesProvider<GeneratorAttributeSyntaxContext> GetViewProvider(
        IncrementalGeneratorInitializationContext context
    )
    {
        var haikaraViewProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
                DefinedStrings.UiAttributeName,
                static (node, _) => node is ClassDeclarationSyntax,
                static (context, _) => context)
            .Where(x =>
            {
                var targetSymbol = x.TargetSymbol;
                var targetTypeSymbol = (ITypeSymbol)targetSymbol;

                var namespaceString = $"{targetSymbol.ContainingNamespace}";
                if (namespaceString == "<global namespace>")
                {
                    return false;
                }
                // Check has HaikaraView
                return targetTypeSymbol.HasInterfaceTypeName(DefinedStrings.ViewInterfaceName);
            });

        return haikaraViewProvider;
    }
    
    public static IncrementalValuesProvider<GeneratorAttributeSyntaxContext> GetStyleProvider(
        IncrementalGeneratorInitializationContext context
    )
    {
        var haikaraViewProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
                DefinedStrings.UiAttributeName,
                static (node, _) => node is ClassDeclarationSyntax,
                static (context, _) => context)
            .Where(x =>
            {
                var targetSymbol = x.TargetSymbol;
                var targetTypeSymbol = (ITypeSymbol)targetSymbol;
                // Check has HaikaraView
                return targetTypeSymbol.HasInterfaceTypeName(DefinedStrings.StyleInterfaceName);
            });

        return haikaraViewProvider;
    }
}