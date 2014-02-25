using Autofac;
using Bootstrap.Autofac;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Tests.Extensions.Containers.Autofac
{
    public class TestAutofacRegistration: IAutofacRegistration
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<LoadedAssemblyProvider>().As<IBootstrapperAssemblyProvider>();
            containerBuilder.RegisterType<RegistrationHelper>().As<IRegistrationHelper>();
            containerBuilder.RegisterType<BootstrapperContainerExtensionOptions>().As<IBootstrapperContainerExtensionOptions>();
            containerBuilder.RegisterType<AutofacExtension>();
        }
    }
}
