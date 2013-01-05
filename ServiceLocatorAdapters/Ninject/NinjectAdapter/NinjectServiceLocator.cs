using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using Ninject;

namespace CommonServiceLocator.NinjectAdapter
{
    public class NinjectServiceLocator: ServiceLocatorImplBase
    {
        private readonly IKernel kernel;

        public NinjectServiceLocator(IKernel kernel)
        {
            this.kernel = kernel;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return string.IsNullOrEmpty(key) ? kernel.Get(serviceType, (string)null) : kernel.Get(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}
