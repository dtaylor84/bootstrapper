using Bootstrap.Extensions.Containers;
using Bootstrap.Windsor;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    public class TestWindsorRegistration: IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.Register(Component.For<IRegistrationHelper>().ImplementedBy<RegistrationHelper>());
            container.Register(Component.For<IBootstrapperContainerExtensionOptions>().ImplementedBy<BootstrapperContainerExtensionOptions>());
            container.Register(Component.For<WindsorExtension>().ImplementedBy<WindsorExtension>());
        }
    }
}
