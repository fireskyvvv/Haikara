using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Haikara.Shared;

public static class StyleFileReader
{
    public static bool TryGetStyleFileInfo(string assetGuid, string assetFullPath, out StyleFileInfo styleFileInfo)
    {
        styleFileInfo = null;
        if (assetFullPath == null)
        {
            return false;
        }

        var styleSheetContents = File.ReadAllText(assetFullPath);
        styleFileInfo = new StyleFileInfo()
        {
            StyleFileGuid = assetGuid,
            StyleFilePath = assetFullPath,
            UsedClassNames = FindUsedClassNames(styleSheetContents).ToList()
        };

        return true;
    }

    private static IEnumerable<string> FindUsedClassNames(string styleSheetContents)
    {
        const string selectorPattern = @"(?<!\d)\.[\w-]+";
        var matches = Regex.Matches(styleSheetContents, selectorPattern);
        
        return matches.Cast<Match>()
            .Select(x=>x.Value.TrimStart('.'))
            .Distinct()
            .ToList();
    }
}