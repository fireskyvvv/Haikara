using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Haikara.Shared;
using Microsoft.CodeAnalysis;
using SourceGenerator.Utils;

namespace SourceGenerator.Providers;

public static class GenerationSourceProvider
{
    public static IncrementalValuesProvider<GenerationSource> GetViewGenerationSourcesProvider(
        IncrementalValuesProvider<GeneratorAttributeSyntaxContext> haikaraUIProvider
    )
    {
        return haikaraUIProvider
            .SelectMany((context, _) =>
            {
                var targetSymbol = (ITypeSymbol)context.TargetSymbol;

                var builder = ImmutableArray.CreateBuilder<GenerationSource>();
                var csFilePath = string.Empty;
                var uiAssetGuid = string.Empty;
                foreach (var syntaxReference in targetSymbol.DeclaringSyntaxReferences)
                {
                    csFilePath = syntaxReference.SyntaxTree.FilePath;
                    if (UIAssetGuidReader.TryGetUxmlGuid(csFilePath, out var guid))
                    {
                        uiAssetGuid = guid;
                        break;
                    }
                }

                var isInheritsViewBase = targetSymbol.IsInheritsClass(DefinedStrings.ViewTypeName);

                if (!string.IsNullOrWhiteSpace(csFilePath) && !string.IsNullOrWhiteSpace(uiAssetGuid))
                {
                    builder.Add(
                        new GenerationSource(
                            UiContext: context,
                            CsFilePath: csFilePath,
                            AssetGuid: uiAssetGuid,
                            IsNeedOverrideAttribute: isInheritsViewBase
                        )
                    );
                }

                return builder.ToImmutable();
            });
    }
    
    public static IncrementalValuesProvider<GenerationSource> GetStyleGenerationSourcesProvider(
        IncrementalValuesProvider<GeneratorAttributeSyntaxContext> haikaraUIProvider
    )
    {
        return haikaraUIProvider
            .SelectMany((context, _) =>
            {
                var targetSymbol = (ITypeSymbol)context.TargetSymbol;

                var builder = ImmutableArray.CreateBuilder<GenerationSource>();
                var csFilePath = string.Empty;
                var uiAssetGuid = string.Empty;
                foreach (var syntaxReference in targetSymbol.DeclaringSyntaxReferences)
                {
                    csFilePath = syntaxReference.SyntaxTree.FilePath;
                    if (UIAssetGuidReader.TryGetUssGuid(csFilePath, out var guid))
                    {
                        uiAssetGuid = guid;
                        break;
                    }
                }

                var isInheritsStyleBase = targetSymbol.IsInheritsClass(DefinedStrings.StyleBaseTypeName);

                if (!string.IsNullOrWhiteSpace(csFilePath) && !string.IsNullOrWhiteSpace(uiAssetGuid))
                {
                    builder.Add(
                        new GenerationSource(
                            UiContext: context,
                            CsFilePath: csFilePath,
                            AssetGuid: uiAssetGuid,
                            IsNeedOverrideAttribute: isInheritsStyleBase
                        )
                    );
                }

                return builder.ToImmutable();
            });
    }    
}

