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
public class ClickCommandAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        RuleDefines.ErrorClickCommandHasNoEventBaseArgument
    );

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
    }

    private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not MethodDeclarationSyntax methodDeclaration)
        {
            return;
        }

        var methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
        if (methodSymbol == null)
        {
            return;
        }

        var hasClickCommandAttribute = methodSymbol.GetAttributes().Any(attr =>
            attr.AttributeClass?.ToDisplayString() == DefinedStrings.ClickCommandAttributeTypeName);
        if (!hasClickCommandAttribute)
        {
            return;
        }

        // A method with the ClickCommand attribute must have exactly one parameter.
        if (methodSymbol.Parameters.Length != 1)
        {
            var diagnostic = Diagnostic.Create(
                RuleDefines.ErrorClickCommandHasNoEventBaseArgument,
                methodDeclaration.Identifier.GetLocation(),
                methodSymbol.Name
            );
            context.ReportDiagnostic(diagnostic);
            return;
        }

        var parameterSymbol = methodSymbol.Parameters[0];
        var parameterType = parameterSymbol.Type;

        // The parameter of a method with the ClickCommand attribute must be of type EventBase.
        if (parameterType.ToDisplayString() != DefinedStrings.UnityEventBaseTypeName)
        {
            var diagnostic = Diagnostic.Create(
                RuleDefines.ErrorClickCommandHasNoEventBaseArgument,
                methodDeclaration.Identifier.GetLocation(),
                methodSymbol.Name
            );
            context.ReportDiagnostic(diagnostic);
        }
    }
}