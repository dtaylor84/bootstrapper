using Bootstrapper.WindsorExtension;
using Castle.Windsor;

namespace Bootstrapper.Tests.Extensions.Containers.Windsor
{
    public class TestWindsorRegistration: IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.AddComponent<WindsorContainerExtension, WindsorContainerExtension>();
        }
    }
}
