using Microsoft.Practices.Unity;

namespace Bootstrap.Unity
{
    public interface IUnityRegistration
    {
        void Register(IUnityContainer container);
    }
}
