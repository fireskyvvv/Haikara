namespace SourceGenerator.Analyzers.Rules;

public enum RuleId
{
    ErrorNotMatchedFileNameAndClassName = 1,
    ErrorMultipleHaikaraUiAttributeClass,
    ErrorNestedHaikaraUiAttributeClass,
    WarningMissingRelatedUxmlFile,
    ErrorClickCommandHasNoEventBaseArgument,
    WarningHaikaraUiHasNotImplementedValidInterface,
}