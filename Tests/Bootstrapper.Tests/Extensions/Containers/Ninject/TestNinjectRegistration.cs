using Bootstrap.Ninject;
using Ninject;

namespace Bootstrap.Tests.Extensions.Containers.Ninject
{
    public class TestNinjectRegistration: INinjectRegistration
    {
        public void Register(IKernel container)
        {
            container.Bind<NinjectExtension>().To<NinjectExtension>();
        }
    }
}
