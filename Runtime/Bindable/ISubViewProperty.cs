using System.Collections.Generic;
using Haikara.Runtime.View;

namespace Haikara.Runtime.Bindable
{
    public interface ISubViewProperty
    {
        public IEnumerable<IHaikaraView> GetViewInstances();
    }
}