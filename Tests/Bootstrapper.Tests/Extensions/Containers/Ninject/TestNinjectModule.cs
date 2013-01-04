using Bootstrap.Tests.Extensions.TestImplementations;
using Ninject.Modules;

namespace Bootstrap.Tests.Extensions.Containers.Ninject
{
    public class TestNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITestInterface>().To<TestImplementation>();
        }
    }
}
