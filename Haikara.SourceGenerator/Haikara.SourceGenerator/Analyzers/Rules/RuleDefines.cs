#pragma warning disable RS2008

using Microsoft.CodeAnalysis;

namespace SourceGenerator.Analyzers.Rules;

internal static class RuleDefines
{
    public static DiagnosticDescriptor ErrorNotMatchedFileNameAndClassName => new(
        id: RuleId.ErrorNotMatchedFileNameAndClassName.ToIdString(),
        title: "Haikara Code Analyzer",
        messageFormat: new LocalizableResourceString(
            nameOfLocalizableResource: nameof(Resources.ErrorNotMatchedFileNameAndClassName),
            resourceManager: Resources.ResourceManager,
            resourceSource: typeof(Resources)
        ),
        category: "Haikara.Core.View",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static DiagnosticDescriptor ErrorMultipleHaikaraViewAttributeClass => new(
        id: RuleId.ErrorMultipleHaikaraUiAttributeClass.ToIdString(),
        title: "Haikara Code Analyzer",
        messageFormat: new LocalizableResourceString(
            nameOfLocalizableResource: nameof(Resources.ErrorMultipleHaikaraUiAttributeClass),
            resourceManager: Resources.ResourceManager,
            resourceSource: typeof(Resources)
        ),
        category: "Haikara.Core.View",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static DiagnosticDescriptor ErrorNestedHaikaraViewAttributeClass => new(
        id: RuleId.ErrorNestedHaikaraUiAttributeClass.ToIdString(),
        title: "Haikara Code Analyzer",
        messageFormat: new LocalizableResourceString(
            nameOfLocalizableResource: nameof(Resources.ErrorNestedHaikaraUiAttributeClass),
            resourceManager: Resources.ResourceManager,
            resourceSource: typeof(Resources)
        ),
        category: "Haikara.Core.View",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static DiagnosticDescriptor WarningMissingRelatedUxmlFile => new(
        id: RuleId.WarningMissingRelatedUxmlFile.ToIdString(),
        title: "Haikara Code Analyzer",
        messageFormat: new LocalizableResourceString(
            nameOfLocalizableResource: nameof(Resources.WarningMissingRelatedUxmlFile),
            resourceManager: Resources.ResourceManager,
            resourceSource: typeof(Resources)
        ),
        category: "Haikara.Core.View",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static DiagnosticDescriptor ErrorClickCommandHasNoEventBaseArgument => new(
        id: RuleId.ErrorClickCommandHasNoEventBaseArgument.ToIdString(),
        title: "Haikara Code Analyzer",
        messageFormat: new LocalizableResourceString(
            nameOfLocalizableResource: nameof(Resources.ErrorClickCommandHasNoEventBaseArgument),
            resourceManager: Resources.ResourceManager,
            resourceSource: typeof(Resources)
        ),
        category: "Haikara.Core.View",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static DiagnosticDescriptor WarningHaikaraViewHasNotImplementedIView => new(
        id: RuleId.WarningHaikaraUiHasNotImplementedValidInterface.ToIdString(),
        title: "Haikara Code Analyzer",
        messageFormat: new LocalizableResourceString(
            nameOfLocalizableResource: nameof(Resources.WarningHaikaraUiHasNotImplementedValidInterface),
            resourceManager: Resources.ResourceManager,
            resourceSource: typeof(Resources)
        ),
        category: "Haikara.Core.View",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}