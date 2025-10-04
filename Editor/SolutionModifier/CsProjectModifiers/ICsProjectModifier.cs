using System.Xml.Linq;

namespace Haikara.Editor.SolutionModifier.CsProjectModifiers
{
    internal interface ICsProjectModifier
    {
        public void Modify(XDocument doc, string csProjectPath, out ModifyResult modifyResult);
    }
}