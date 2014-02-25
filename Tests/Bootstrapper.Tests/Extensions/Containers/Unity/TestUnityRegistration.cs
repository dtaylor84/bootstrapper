using Bootstrap.Extensions.Containers;
using Bootstrap.Unity;
using Microsoft.Practices.Unity;

namespace Bootstrap.Tests.Extensions.Containers.Unity
{
    public class TestUnityRegistration: IUnityRegistration
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IBootstrapperAssemblyProvider, LoadedAssemblyProvider>();
            container.RegisterType<IRegistrationHelper, RegistrationHelper>();
            container.RegisterType<IBootstrapperContainerExtensionOptions, BootstrapperContainerExtensionOptions>();
            container.RegisterType<UnityExtension, UnityExtension>();
        }
    }
}
