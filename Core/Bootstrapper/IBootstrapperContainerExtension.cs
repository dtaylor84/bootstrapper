using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bootstrapper
{
    public interface IBootstrapperContainerExtension: IBootstrapperExtension
    {
        IAssemblyCollector LookForRegistrations { get; }
        IAssemblyCollector LookForMaps { get; }
        IAssemblyCollector LookForStartupTasks { get; }
    }
}
