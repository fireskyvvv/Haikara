using System;
using System.IO;

namespace Haikara.Shared;

public static class UIAssetGuidReader
{
    public static bool TryGetUxmlGuid(string csPath, out string guid)
    {
        return TryGetUIAssetGuid(csPath, "uxml", out guid);
    }

    public static bool TryGetUssGuid(string csPath, out string guid)
    {
        return TryGetUIAssetGuid(csPath, "uss", out guid);
    }

    private static bool TryGetUIAssetGuid(string csPath, string uiAssetExtension, out string guid)
    {
        guid = string.Empty;
        var uiAssetPath = Path.ChangeExtension(csPath, uiAssetExtension);
        var metaFilePath = uiAssetPath + ".meta";

        if (!File.Exists(metaFilePath))
        {
            return false;
        }

        var lines = File.ReadAllLines(metaFilePath);

        foreach (var line in lines)
        {
            // 先頭の空白を無視して "guid:" で始まるかチェック
            if (!line.Trim().StartsWith("guid:"))
            {
                continue;
            }

            guid = line.Split(':')[1].Trim();
            return true;
        }

        return false;
    }
}