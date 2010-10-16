using Castle.Windsor;

namespace Bootstrap.WindsorExtension
{
    public interface IWindsorRegistration
    {
        void Register(IWindsorContainer container);
    }
}
