using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.ViewTemplates;

internal partial class ViewTemplate
{
    private static IEnumerable
        <(string actionNameOrId, string inputTypeString, string throwIfNotFound, string methodName)>
        GetInputActionCallbackInfo(
            GenerationSource generationSource
        )
    {
        var methodSymbols = generationSource.GetTypeSymbol()
            .GetMembers()
            .OfType<IMethodSymbol>();
        foreach (var methodSymbol in methodSymbols)
        {
            foreach (var attributeData in methodSymbol.GetAttributes())
            {
                if (attributeData.AttributeClass == null)
                {
                    continue;
                }

                if (attributeData.AttributeClass.OriginalDefinition.ToDisplayString() !=
                    DefinedStrings.InputActionCallbackTypeName)
                {
                    continue;
                }

                var attributeSyntax = attributeData.ApplicationSyntaxReference?.GetSyntax() as AttributeSyntax;
                if (attributeSyntax == null)
                {
                    continue;
                }

                if (attributeSyntax.ArgumentList == null)
                {
                    continue;
                }

                var actionNameOrId = "";
                var inputTypeString = "";

                foreach (var argument in attributeSyntax.ArgumentList.Arguments)
                {
                    if (argument.NameColon != null)
                    {
                        var nameColonValue = argument.NameColon.Name.Identifier.ValueText;
                        if (nameColonValue == "actionNameOrId")
                        {
                            actionNameOrId = $" {argument.Expression.ToString()}";
                        }

                        if (argument.NameColon.Name.Identifier.ValueText == "inputType")
                        {
                            var symbolInfo = generationSource.UiContext.SemanticModel.GetSymbolInfo(argument.Expression);
                            if (symbolInfo.Symbol is IFieldSymbol fieldSymbol)
                            {
                                inputTypeString = fieldSymbol.ToDisplayString();
                            }
                        }
                    }
                }

                var throwIfNotFound = "false";
                for (var argumentIndex = 0;
                     argumentIndex < attributeSyntax.ArgumentList.Arguments.Count;
                     argumentIndex++)
                {
                    var argument = attributeSyntax.ArgumentList.Arguments[argumentIndex];

                    if (string.IsNullOrEmpty(actionNameOrId) && argumentIndex == 0)
                    {
                        actionNameOrId = argument.Expression.ToString();
                        continue;
                    }

                    if (string.IsNullOrEmpty(inputTypeString) && argumentIndex == 1)
                    {
                        var symbolInfo = generationSource.UiContext.SemanticModel.GetSymbolInfo(argument.Expression);
                        if (symbolInfo.Symbol is IFieldSymbol fieldSymbol)
                        {
                            inputTypeString = fieldSymbol.ToDisplayString();
                        }
                    }

                    if (argument.NameEquals == null)
                    {
                        continue;
                    }

                    var argumentName = argument.NameEquals.Name.ToString();

                    if (argumentName.StartsWith("ThrowIfNotFound"))
                    {
                        throwIfNotFound = argument.Expression.ToString();
                    }
                }

                yield return (actionNameOrId, inputTypeString, throwIfNotFound, methodSymbol.Name);
            }
        }
    }
}