using Microsoft.CodeAnalysis;

namespace SourceGenerator;

public record GenerationSource(
    GeneratorAttributeSyntaxContext UiContext,
    string CsFilePath,
    string AssetGuid,
    bool IsNeedOverrideAttribute)
{
    public GeneratorAttributeSyntaxContext UiContext { get; } = UiContext;
    public string CsFilePath { get; } = CsFilePath;
    public string AssetGuid { get; } = AssetGuid;

    public bool IsNeedOverrideAttribute { get; } = IsNeedOverrideAttribute;
}