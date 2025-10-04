using Microsoft.CodeAnalysis;
using SourceGenerator.Utils;

namespace SourceGenerator.Providers;

public static class AssemblyNameAndFilterProvider
{
    public static IncrementalValueProvider<string?> Get(
        IncrementalGeneratorInitializationContext context)
    {
        var assemblyNameAndFilterProvider = context.CompilationProvider
            .Select((compilation, _) =>
            {
                // Ignore Unity assembly
                if (compilation.AssemblyName is "Assembly-CSharp" or "Assembly-CSharp-Editor")
                {
                    return null;
                }

                foreach (var referencedAssemblyName in compilation.ReferencedAssemblyNames)
                {
                    if (referencedAssemblyName.Name == DefinedStrings.RuntimeNamespace)
                    {
                        return compilation.AssemblyName;
                    }
                }

                return null;
            });

        return assemblyNameAndFilterProvider;
    }
}