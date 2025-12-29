using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.ViewTemplates;

internal partial class ViewTemplate
{
    private static IEnumerable<(string elementName, string indexString, string findTypeString, string methodName, string
            eventTypeString)>
        GetCallbackInfo(
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
                    DefinedStrings.ClickCallbackTypeName)
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

                var elementNameString = "";
                var eventTypeString = "";

                foreach (var argument in attributeSyntax.ArgumentList.Arguments)
                {
                    if (argument.NameColon != null)
                    {
                        var nameColonValue = argument.NameColon.Name.Identifier.ValueText;
                        if (nameColonValue == "targetElementName")
                        {
                            elementNameString = $" {argument.Expression.ToString()}";
                        }

                        if (argument.NameColon.Name.Identifier.ValueText == "eventType")
                        {
                            if (argument.Expression is TypeOfExpressionSyntax typeOfExpressionSyntax)
                            {
                                var typeInfo =
                                    generationSource.UiContext.SemanticModel.GetTypeInfo(typeOfExpressionSyntax.Type);
                                if (typeInfo.Type != null)
                                {
                                    eventTypeString = typeInfo.Type.ToDisplayString();
                                }
                            }
                        }
                    }
                }

                var indexString = "-1";
                var findTypeString = $"{DefinedStrings.ElementFindTypeTypeName}.First";
                for (var argumentIndex = 0;
                     argumentIndex < attributeSyntax.ArgumentList.Arguments.Count;
                     argumentIndex++)
                {
                    var argument = attributeSyntax.ArgumentList.Arguments[argumentIndex];

                    if (string.IsNullOrEmpty(elementNameString) && argumentIndex == 0)
                    {
                        elementNameString = argument.Expression.ToString();
                        continue;
                    }

                    if (string.IsNullOrEmpty(eventTypeString) && argumentIndex == 1)
                    {
                        if (argument.Expression is TypeOfExpressionSyntax typeOfExpressionSyntax)
                        {
                            var typeInfo =
                                generationSource.UiContext.SemanticModel.GetTypeInfo(typeOfExpressionSyntax.Type);
                            if (typeInfo.Type != null)
                            {
                                eventTypeString = typeInfo.Type.ToDisplayString();
                            }
                        }
                    }

                    if (argument.NameEquals == null)
                    {
                        continue;
                    }

                    var argumentName = argument.NameEquals.Name.ToString();

                    if (argumentName.StartsWith("ElementIndex"))
                    {
                        indexString = argument.Expression.ToString();
                    }
                    else if (argumentName.StartsWith("FindType"))
                    {
                        findTypeString = argument.Expression.ToString();
                    }
                }

                yield return (elementNameString, indexString, findTypeString, methodSymbol.Name, eventTypeString);
            }
        }
    }
}