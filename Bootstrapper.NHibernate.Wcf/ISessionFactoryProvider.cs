using NHibernate;

namespace Bootstrap.NHibernate.Wcf
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory GetSessionFactory();
    }
}