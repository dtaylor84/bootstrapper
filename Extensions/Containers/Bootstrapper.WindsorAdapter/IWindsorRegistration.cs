using Castle.Windsor;

namespace Bootstrapper.WindsorExtension
{
    public interface IWindsorRegistration
    {
        void Register(IWindsorContainer container);
    }
}
