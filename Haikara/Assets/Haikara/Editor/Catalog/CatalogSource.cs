using System.Collections.Generic;

namespace Haikara.Editor.Catalog
{
    public class CatalogSource
    {
        public List<string> UxmlFileGuids { get; } = new();
        public List<string> UssFileGuids { get; } = new();
    }
}