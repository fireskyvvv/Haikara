using System.Linq;
using Microsoft.CodeAnalysis;
using Haikara.Shared;

namespace SourceGenerator.Utils;

public static class HaikaraViewTypeUtil
{
    public static bool IsInheritsClass(this ITypeSymbol symbol, string classTypeName)
    {
        var currentBaseType = symbol.BaseType;

        while (currentBaseType != null)
        {
            if (currentBaseType.OriginalDefinition.ToDisplayString() == classTypeName)
            {
                return true;
            }

            currentBaseType = currentBaseType.BaseType;
        }

        return false;
    }

    public static bool HasInterfaceTypeName(this ITypeSymbol symbol, string interfaceTypeName)
    {
        var currentType = symbol;

        while (currentType != null)
        {
            if (currentType.Interfaces.Any(
                    interfaceSymbol =>
                        interfaceSymbol.OriginalDefinition.ToDisplayString() == interfaceTypeName
                )
               )
            {
                return true;
            }


            currentType = currentType.BaseType;
        }

        return false;
    }
}