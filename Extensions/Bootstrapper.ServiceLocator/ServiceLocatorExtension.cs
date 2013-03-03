using Bootstrap.Extensions;

namespace Bootstrap.Locator
{
    public class ServiceLocatorExtension: IBootstrapperExtension
    {
        public void Run()
        {
            Bootstrapper.ContainerExtension.SetServiceLocator();
        }

        public void Reset()
        {
            Bootstrapper.ContainerExtension.ResetServiceLocator();
        }
    }
}
