using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceGenerator.Analyzers.Rules;
using SourceGenerator.Utils;

namespace SourceGenerator.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ViewNamedTypeAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        RuleDefines.WarningHaikaraViewHasNotImplementedIView
    );

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeNamedTypeSymbol, SymbolKind.NamedType);
    }

    private static void AnalyzeNamedTypeSymbol(SymbolAnalysisContext context)
    {
        var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

        if (namedTypeSymbol.TypeKind != TypeKind.Class)
        {
            return;
        }

        var targetAttribute = namedTypeSymbol.GetAttributes()
            .FirstOrDefault(attr => attr.AttributeClass?.ToDisplayString() == DefinedStrings.UiAttributeName);

        if (targetAttribute == null)
        {
            return;
        }

        // Warn if a class with the HaikaraUIAttribute does not implement IHaikaraView or IHaikaraStyle.
        if (!namedTypeSymbol.HasInterfaceTypeName(DefinedStrings.ViewInterfaceName) &&
            !namedTypeSymbol.HasInterfaceTypeName(DefinedStrings.StyleInterfaceName)
           )
        {
            var diagnostic = Diagnostic.Create(
                RuleDefines.WarningHaikaraViewHasNotImplementedIView,
                namedTypeSymbol.Locations[0]
            );
            context.ReportDiagnostic(diagnostic);
        }
    }
}