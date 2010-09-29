using Bootstrapper.UnityExtension;
using Microsoft.Practices.Unity;
using UnityContainerExtension = Bootstrapper.UnityExtension.UnityContainerExtension;

namespace Bootstrapper.Tests.Extensions.Containers.Unity
{
    public class TestUnityRegistration: IUnityRegistration
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<UnityContainerExtension, UnityContainerExtension>();
        }
    }
}
