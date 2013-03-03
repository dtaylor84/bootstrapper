using SimpleInjector;

namespace Bootstrap.SimpleInjector
{
    public interface ISimpleInjectorRegistration
    {
        void Register(Container container);
    }
}