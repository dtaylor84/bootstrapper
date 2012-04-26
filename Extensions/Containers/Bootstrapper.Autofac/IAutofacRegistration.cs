using Autofac;

namespace Bootstrap.Autofac
{
    public interface IAutofacRegistration
    {
        void Register(ContainerBuilder containerBuilder);
    }
}
