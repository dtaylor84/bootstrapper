using Bootstrap.Extensions;

namespace Bootstrap.ServiceLocator
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
