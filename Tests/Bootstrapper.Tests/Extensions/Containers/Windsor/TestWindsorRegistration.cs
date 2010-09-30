using Bootstrap.WindsorExtension;
using Castle.Windsor;

namespace Bootstrap.Tests.Extensions.Containers.Windsor
{
    public class TestWindsorRegistration: IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            container.AddComponent<WindsorContainerExtension, WindsorContainerExtension>();
        }
    }
}
