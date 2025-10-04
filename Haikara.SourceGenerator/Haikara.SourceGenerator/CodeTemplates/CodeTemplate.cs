namespace SourceGenerator.CodeTemplates;

public abstract class CodeTemplate<T>
{
    public abstract string BuildCodeString(T generationSource);
}
