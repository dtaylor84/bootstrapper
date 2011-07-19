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
#if (DEBUG)
            NHibernateProfiler.Initialize();
#endif

            return sessionFactory ?? 
                (sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(
                        connectionStringProvider.GetConnectionString(CafConfigKeys.CcsConnectionStringKey)))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<IngestionErrorMap>())
                    .BuildSessionFactory());
        }
    }
}