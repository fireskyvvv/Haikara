namespace SourceGenerator.Analyzers.Rules;

public static class RuleIdExtension
{
    public static string ToIdString(this RuleId id)
    {
        var idNum = (int)id;
        return $"HK{idNum:0000}";
    }
}