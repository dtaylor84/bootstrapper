using Ninject;

namespace Bootstrap.Ninject
{
    public interface INinjectRegistration
    {
        void Register(IKernel container);
    }
}
