using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace SourceGenerator.Utils;

public static class AssetFindUtil
{
    public static bool GetTargetAssetRawPath(ITypeSymbol typeSymbol, string assetExtension,
        out string assetRawPath)
    {
        var syntaxRef = typeSymbol.DeclaringSyntaxReferences.FirstOrDefault();
        assetRawPath = "";
        if (syntaxRef == null)
        {
            return false;
        }

        assetRawPath = Path.ChangeExtension(syntaxRef.SyntaxTree.FilePath, $".{assetExtension}");
        if (string.IsNullOrEmpty(assetRawPath))
        {
            return false;
        }

        return true;
    }
}