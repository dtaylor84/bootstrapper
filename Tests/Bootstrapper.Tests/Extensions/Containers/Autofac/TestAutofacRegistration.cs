using Autofac;
using Bootstrap.Autofac;

namespace Bootstrap.Tests.Extensions.Containers.Autofac
{
    public class TestAutofacRegistration: IAutofacRegistration
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AutofacExtension>();
        }
    }
}
