using Bootstrap.StructureMap;
using Calamos.Operations.Diem.Data.Orm;
using NHibernate;
using StructureMap;

namespace Calamos.Operations.Diem.Infrastructure
{
    public class RegisterDataOrm : IStructureMapRegistration
    {
        public void Register(IContainer container)
        {
            container.Configure(c =>
                                    {
                                        c.ForSingletonOf<ISessionFactoryProvider>().Use<SessionFactoryProvider>();
                                        c.For<ISessionFactory>().Use(
                                            cn => container.GetInstance<ISessionFactoryProvider>().GetSessionFactory());
                                        c.For<ISession>().Use(NHibernateSessionContext.CurrentSession);
                                    });
        }
    }
}