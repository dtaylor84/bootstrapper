using System;

namespace Bootstrap.NHibernate.Wcf
{
    public class SessionPerCallInstanceProvider : IInstanceProvider
    {
        private readonly Type serviceType;

        static SessionPerCallInstanceProvider()
        {
            new Initializer().Initialize();
        }

        public SessionPerCallInstanceProvider(Type serviceType)
        {
            this.serviceType = serviceType;
        }

        #region IInstanceProvider Members

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            var nHibernateContextManager = instanceContext.Extensions.Find<NHibernateContextManager>();
            if (nHibernateContextManager == null) instanceContext.Extensions.Add(new NHibernateContextManager());
            return ServiceLocator.Current.GetInstance(serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var nHibernateContextManager = instanceContext.Extensions.Find<NHibernateContextManager>();
            if (nHibernateContextManager != null) instanceContext.Extensions.Remove(nHibernateContextManager);
        }

        #endregion IInstanceProvider Members
    }
}