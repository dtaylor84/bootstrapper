using Bootstrap.Unity;
using Microsoft.Practices.Unity;

namespace Bootstrap.Tests.Extensions.Containers.Unity
{
    public class TestUnityRegistration: IUnityRegistration
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<UnityExtension, UnityExtension>();
        }
    }
}
