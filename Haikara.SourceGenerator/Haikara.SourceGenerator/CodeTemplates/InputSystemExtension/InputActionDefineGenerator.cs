using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.InputSystemExtension;

public class InputActionDefineGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.ForAttributeWithMetadataName(
            DefinedStrings.InputActionDefineTypeName,
            static (node, _) => node is ClassDeclarationSyntax,
            static (context, _) => context
        );
    }
}