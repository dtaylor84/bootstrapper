using System;

namespace Bootstrap.NHibernate.Wcf
{
    public static class NHibernateSessionContext
    {
        public static ISession CurrentSession()
        {
            // Get the WCF InstanceContext:
            var contextManager = OperationContext.Current.InstanceContext.Extensions.Find<NHibernateContextManager>();
            if (contextManager == null)
            {
                throw new InvalidOperationException(
                    @"There is no context manager available.
Check whether the NHibernateContextManager is added as InstanceContext extension.
Also, this Session Provider only makes sense in a WCF context.");
            }

            return contextManager.Session ??
                   (contextManager.Session = ServiceLocator.Current.GetInstance<ISessionFactory>().OpenSession());
        }
    }
}