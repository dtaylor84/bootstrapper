using Bootstrap.UnityExtension;
using Microsoft.Practices.Unity;
using UnityContainerExtension = Bootstrap.UnityExtension.UnityContainerExtension;

namespace Bootstrap.Tests.Extensions.Containers.Unity
{
    public class TestUnityRegistration: IUnityRegistration
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<UnityContainerExtension, UnityContainerExtension>();
        }
    }
}
