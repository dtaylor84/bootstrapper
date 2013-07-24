using Bootstrap.Extensions.Containers;
using Bootstrap.SimpleInjector;
using SimpleInjector;

namespace Bootstrap.Tests.Extensions.Containers.SimpleInjector
{
    public class TestSimpleInjectorRegistration: ISimpleInjectorRegistration
    {
        public void Register(Container container)
        {
            container.Register<IBootstrapperAssemblyProvider, LoadedAssemblyProvider>();
            container.Register<IRegistrationHelper,RegistrationHelper>();
            container.Register<IBootstrapperContainerExtensionOptions, BootstrapperContainerExtensionOptions>();
            container.Register<SimpleInjectorExtension>();
        }
    }
}
