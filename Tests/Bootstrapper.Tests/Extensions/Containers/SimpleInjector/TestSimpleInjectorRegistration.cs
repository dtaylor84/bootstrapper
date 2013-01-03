using Bootstrap.Extensions.Containers;
using Bootstrap.SimpleInjector;
using SimpleInjector;

namespace Bootstrap.Tests.Extensions.Containers.SimpleInjector
{
    public class TestSimpleInjectorRegistration: ISimpleInjectorRegistration
    {
        public void Register(Container container)
        {
            container.Register<IRegistrationHelper,RegistrationHelper>();
            container.Register<SimpleInjectorExtension>();
        }
    }
}
