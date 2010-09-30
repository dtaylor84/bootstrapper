using Bootstrap.StructureMapExtension;
using StructureMap;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    public class TestStructureMapRegistration: IStructureMapRegistration
    {
        public void Register(IContainer container)
        {
            container.Configure(c => c.For<StructureMapContainerExtension>().Use<StructureMapContainerExtension>());
        }
    }
}
