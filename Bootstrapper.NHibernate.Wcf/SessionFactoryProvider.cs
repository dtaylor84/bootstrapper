using Bootstrap.Extensions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Bootstrap.NHibernate.Wcf
{
    public class SessionFactoryProvider : ISessionFactoryProvider
    {
        private static ISessionFactory sessionFactory;
        private readonly IConnectionStringProvider connectionStringProvider;

        public SessionFactoryProvider(IConnectionStringProvider theConnectionStringProvider)
        {
            connectionStringProvider = theConnectionStringProvider;
        }

        public ISessionFactory GetSessionFactory()
        {
            return sessionFactory ?? 
                (sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(
                        connectionStringProvider.GetConnectionString()))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BootstrapperExtensions>())
                    .BuildSessionFactory());
        }
    }
}