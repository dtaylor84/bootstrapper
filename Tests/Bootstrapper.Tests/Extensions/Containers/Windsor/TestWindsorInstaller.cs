using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    public class TestWindsorInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConfigurationStore>().Instance(store));
        }
    }
}
