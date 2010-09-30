using Microsoft.Practices.Unity;

namespace Bootstrap.UnityExtension
{
    public interface IUnityRegistration
    {
        void Register(IUnityContainer container);
    }
}
