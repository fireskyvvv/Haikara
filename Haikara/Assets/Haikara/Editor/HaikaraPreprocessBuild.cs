using Haikara.Editor.Catalog;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Haikara.Editor
{
    public class HaikaraPreprocessBuild : IPreprocessBuildWithReport
    {
        private const int HaikaraPreprocessCallbackOrder = 1;
        public int callbackOrder => HaikaraPreprocessCallbackOrder;

        public void OnPreprocessBuild(BuildReport report)
        {
            UICatalogGenerator.Regenerate(isPreprocessBuild: true);
        }
    }
}