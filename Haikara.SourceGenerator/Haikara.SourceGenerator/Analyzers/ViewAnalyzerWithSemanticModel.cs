using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerator.Analyzers.Rules;
using SourceGenerator.Utils;

namespace SourceGenerator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ViewAnalyzerWithSemanticModel : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(
            RuleDefines.ErrorClickCommandHasNoEventBaseArgument,
            RuleDefines.ErrorNestedHaikaraViewAttributeClass
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

            compilationContext.RegisterSemanticModelAction(syntaxTreeContext =>
            {
                AnalyzeSyntaxTree(syntaxTreeContext, targetAttributeSymbol);
            });
        });
    }

    private void AnalyzeSyntaxTree(
        SemanticModelAnalysisContext context, ISymbol haikaraViewAttributeSymbol
    )
    {
        var semanticModel = context.SemanticModel;
        var syntaxTree = semanticModel.SyntaxTree;
        var root = syntaxTree.GetRoot(context.CancellationToken);

        // A list of class declarations within the file that are decorated with the HaikaraUIAttribute.
        var haikaraViewAnnotatedClassDeclarationSyntaxList = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Where(x =>
                {
                    var classSymbol = semanticModel.GetDeclaredSymbol(x, context.CancellationToken);
                    var hasHaikaraView = classSymbol != null && classSymbol.GetAttributes().Any(attr =>
                        SymbolEqualityComparer.Default.Equals(attr.AttributeClass, haikaraViewAttributeSymbol));

                    return hasHaikaraView;
                }
            )
            .ToList();

        if (haikaraViewAnnotatedClassDeclarationSyntaxList.Count == 0)
        {
            return;
        }

        // Disallow nested classes with the HaikaraUIAttribute.
        foreach (var classDeclarationSyntax in haikaraViewAnnotatedClassDeclarationSyntaxList
                     .Where(x => x.Parent is ClassDeclarationSyntax)
                )
        {
            var parentClassSyntax = classDeclarationSyntax.Parent as ClassDeclarationSyntax;
            var className = classDeclarationSyntax.Identifier.Text;
            var parentClassName = parentClassSyntax?.Identifier.Text;

            var diagnostic = Diagnostic.Create(
                RuleDefines.ErrorNestedHaikaraViewAttributeClass,
                classDeclarationSyntax.Identifier.GetLocation(),
                className, parentClassName
            );
            context.ReportDiagnostic(diagnostic);
        }

        // Disallow multiple classes with the HaikaraUIAttribute in a single file.
        if (haikaraViewAnnotatedClassDeclarationSyntaxList.Count > 1)
        {
            var filePath = syntaxTree.FilePath;
            var fileName = string.IsNullOrEmpty(filePath) ? "CurrentFile" : System.IO.Path.GetFileName(filePath);

            foreach (var classDeclarationSyntax in haikaraViewAnnotatedClassDeclarationSyntaxList)
            {
                var diagnostic = Diagnostic.Create(RuleDefines.ErrorMultipleHaikaraViewAttributeClass,
                    classDeclarationSyntax.Identifier.GetLocation(),
                    fileName);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}