using Castle.Windsor;

namespace Bootstrap.Windsor
{
    public interface IWindsorRegistration
    {
        void Register(IWindsorContainer container);
    }
}
