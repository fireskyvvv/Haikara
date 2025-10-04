using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerator.Utils;

namespace SourceGenerator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ViewSyntaxNodeAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(
            Rules.RuleDefines.ErrorNotMatchedFileNameAndClassName,
            Rules.RuleDefines.WarningMissingRelatedUxmlFile
        );

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(compilationContext =>
        {
            var targetAttributeSymbol =
                compilationContext.Compilation.GetTypeByMetadataName(DefinedStrings.UiAttributeName);
            if (targetAttributeSymbol == null)
            {
                return;
            }

            compilationContext.RegisterSyntaxNodeAction(
                nodeContext => AnalyzeSyntaxNode(nodeContext, targetAttributeSymbol),
                SyntaxKind.ClassDeclaration);
        });
    }


    private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context, INamedTypeSymbol targetAttributeSymbol)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var hasTargetAttribute = classDeclaration.AttributeLists
            .SelectMany(attributeList => attributeList.Attributes)
            .Any(attributeSyntax =>
            {
                var attributeTypeSymbol = context.SemanticModel.GetTypeInfo(attributeSyntax.Name).Type;
                return SymbolEqualityComparer.Default.Equals(attributeTypeSymbol, targetAttributeSymbol);
            });

        if (!hasTargetAttribute)
        {
            return;
        }

        var typeName = classDeclaration.Identifier.Text;
        var filePath = classDeclaration.SyntaxTree.FilePath;
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        // If the file name and class name do not match, report an error.
        if (!string.Equals(fileNameWithoutExtension, typeName, System.StringComparison.OrdinalIgnoreCase))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                Rules.RuleDefines.ErrorNotMatchedFileNameAndClassName,
                classDeclaration.Identifier.GetLocation(),
                fileNameWithoutExtension, typeName)
            );
        }
    }
}