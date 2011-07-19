using System;
using System.Collections.ObjectModel;

namespace Bootstrap.NHibernate.Wcf
{
    public class SessionPerCallServiceBehavior : Attribute, IServiceBehavior
    {
        #region IServiceBehavior Members

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var ed in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>().SelectMany(cd => cd.Endpoints))
                ed.DispatchRuntime.InstanceProvider = new SessionPerCallInstanceProvider(serviceDescription.ServiceType);
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        #endregion IServiceBehavior Members
    }
}