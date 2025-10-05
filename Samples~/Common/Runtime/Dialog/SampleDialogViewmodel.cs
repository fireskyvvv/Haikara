using Haikara.Samples.Common.Runtime.Dialog;
using Unity.Properties;

namespace Haikara.Samples.Common.Runtime.Dialog
{
    public class SampleDialogViewmodel : SampleDialogViewModelBase
    {
        [CreateProperty] public string Text => "This is Sample Dialog";
    }
}