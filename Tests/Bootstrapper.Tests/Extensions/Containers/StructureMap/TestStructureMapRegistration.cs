using Bootstrap.Extensions.Containers;
using Bootstrap.StructureMap;
using StructureMap;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    public class TestStructureMapRegistration: IStructureMapRegistration
    {
        public void Register(IContainer container)
        {
            container.Configure(c => c.For<IRegistrationHelper>().Use<RegistrationHelper>());
            container.Configure(c => c.For<StructureMapExtension>().Use<StructureMapExtension>());
        }
    }
}
