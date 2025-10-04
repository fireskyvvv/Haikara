using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.ViewTemplates;

internal partial class ViewTemplate : CodeTemplate<GenerationSource>
{
    public override string BuildCodeString(GenerationSource generationSource)
    {
        return PartialViewFrame(generationSource);
    }
}