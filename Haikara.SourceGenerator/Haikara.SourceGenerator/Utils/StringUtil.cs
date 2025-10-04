namespace SourceGenerator.Utils;

public static class StringUtil
{
    public static string GetValidCSharpString(string s, bool allowPeriod = false)
    {
        if (s.Length == 0)
        {
            return "";
        }

        var result = "";
        for (var index = 0; index < s.Length; index++)
        {
            var c = s[index];

            var needReplace = false;
            if (index == 0)
            {
                if (!(char.IsLetter(c) || c == '_'))
                {
                    needReplace = true;
                }
            }
            else
            {
                if (allowPeriod && c == '.')
                {
                    needReplace = false;
                }
                else if (!(char.IsLetterOrDigit(c) || c == '_'))
                {
                    needReplace = true;
                }
            }

            if (needReplace)
            {
                result += "_";
            }
            else
            {
                result += c;
            }
        }

        return result;
    }

    public enum FirstCharMode
    {
        Upper,
        UnderScore
    }

    public static string FormatToCSharpCodeConventions(this string s, FirstCharMode firstCharMode = FirstCharMode.Upper)
    {
        var result = "";
        var nextIsUpper = false;
        foreach (var c in s)
        {
            if (c == '_')
            {
                nextIsUpper = true;
            }
            else
            {
                if (char.IsLower(c) && nextIsUpper)
                {
                    result += char.ToUpper(c);
                    nextIsUpper = false;
                }
                else
                {
                    result += c;
                }
            }
        }

        if (result.Length == 0)
        {
            return "";
        }

        if (firstCharMode == FirstCharMode.Upper)
        {
            var firstChar = char.ToUpper(result[0]);
            result = firstChar + result.Substring(1);
        }

        if (firstCharMode == FirstCharMode.UnderScore)
        {
            result = "_" + result;
        }

        return result;
    }
}