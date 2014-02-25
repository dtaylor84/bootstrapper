using Bootstrap.Extensions.Containers;
using Bootstrap.StructureMap;
using StructureMap;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    public class TestStructureMapRegistration: IStructureMapRegistration
    {
        public void Register(IContainer container)
        {
            container.Configure(c =>
                {
                    c.For<IBootstrapperAssemblyProvider>().Use<LoadedAssemblyProvider>();
                    c.For<IRegistrationHelper>().Use<RegistrationHelper>();
                    c.For<IBootstrapperContainerExtensionOptions>().Use<BootstrapperContainerExtensionOptions>();
                    c.For<StructureMapExtension>().Use<StructureMapExtension>();
                });
        }
    }
}
