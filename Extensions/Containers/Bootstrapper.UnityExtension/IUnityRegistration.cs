using Microsoft.Practices.Unity;

namespace Bootstrapper.UnityExtension
{
    public interface IUnityRegistration
    {
        void Register(IUnityContainer container);
    }
}
