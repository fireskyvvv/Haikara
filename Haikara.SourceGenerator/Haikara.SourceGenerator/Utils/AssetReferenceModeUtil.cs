using System.Linq;
using Microsoft.CodeAnalysis;

namespace SourceGenerator.Utils;

public class AssetReferenceModeUtil
{
    public enum AssetReferenceMode
    {
        Resource,
        AssetPath,
        Custom
    }

    public static AssetReferenceMode GetReferenceModeFromHaikaraUiAttribute(ITypeSymbol typeSymbol)
    {
        var attr = typeSymbol.GetAttributes()
            .FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == DefinedStrings.UiAttributeName);
        if (attr == null)
        {
            return AssetReferenceMode.Resource;
        }

        var attrValue = attr.NamedArguments.FirstOrDefault(x => x.Key == "ReferenceMode").Value;

        if (attrValue is { Value: not null, Kind: TypedConstantKind.Enum } &&
            attrValue.Type?.ToDisplayString() == DefinedStrings.AssetReferenceMode)
        {
            var referenceModeInt = (int)attrValue.Value;
            return (AssetReferenceMode)referenceModeInt;
        }

        return AssetReferenceMode.Resource;
    }
}