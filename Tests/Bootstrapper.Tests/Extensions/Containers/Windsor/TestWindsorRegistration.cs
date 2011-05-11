using Bootstrap.Windsor;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    public class TestWindsorRegistration: IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.Register(Component.For<WindsorExtension>().ImplementedBy<WindsorExtension>());
        }
    }
}
