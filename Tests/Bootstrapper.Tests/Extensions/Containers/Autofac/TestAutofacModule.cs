using Autofac;
using Bootstrap.Tests.Extensions.TestImplementations;

namespace Bootstrap.Tests.Extensions.Containers.Autofac
{
    public class TestAutofacModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TestImplementation>().As<ITestInterface>();
        }
    }
}
