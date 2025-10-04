using Microsoft.CodeAnalysis;
using SourceGenerator.Utils;

namespace SourceGenerator;

public static class GenerationSourceUtil
{
    public static ITypeSymbol GetTypeSymbol(this GenerationSource self)
    {
        return (ITypeSymbol)self.UiContext.TargetSymbol;
    }

    public static string ReferenceModeString(this GenerationSource self)
    {
        var referenceMode = AssetReferenceModeUtil.GetReferenceModeFromHaikaraUiAttribute(self.GetTypeSymbol());
        return $"{DefinedStrings.AssetReferenceMode}.{referenceMode}";
    }

    public static string ReturnType(this GenerationSource self, string returnTypeString)
    {
        return self.IsNeedOverrideAttribute ? $"override {returnTypeString}" : returnTypeString;
    }
}