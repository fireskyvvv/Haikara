using System;
using System.Collections.Generic;

namespace Haikara.Shared
{
    [Serializable]
    public class StyleFileInfo
    {
        public string StyleFilePath { get; set; } = "";
        public string StyleFileGuid { get; set; } = "";
        public List<string> UsedClassNames { get; set; } = new();
    }
}