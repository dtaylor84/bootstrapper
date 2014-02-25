using Bootstrap.Extensions.Containers;
using Bootstrap.Ninject;
using Ninject;

namespace Bootstrap.Tests.Extensions.Containers.Ninject
{
    public class TestNinjectRegistration: INinjectRegistration
    {
        public void Register(IKernel container)
        {
            container.Bind<IBootstrapperAssemblyProvider>().To<LoadedAssemblyProvider>();
            container.Bind<IRegistrationHelper>().To<RegistrationHelper>();
            container.Bind<IBootstrapperContainerExtensionOptions>().To<BootstrapperContainerExtensionOptions>();
            container.Bind<NinjectExtension>().To<NinjectExtension>();
        }
    }
}
