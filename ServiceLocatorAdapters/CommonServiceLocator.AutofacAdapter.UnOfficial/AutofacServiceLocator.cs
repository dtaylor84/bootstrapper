using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Microsoft.Practices.ServiceLocation;

namespace CommonServiceLocator.AutofacAdapter.Unofficial
{
    public sealed class AutofacServiceLocator : ServiceLocatorImplBase
    {
        private readonly IComponentContext container;

        public AutofacServiceLocator(IComponentContext container)
        {
            this.container = container;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key != null ? container.ResolveNamed(key, serviceType) : container.Resolve(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return ((IEnumerable)container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType))).Cast<object>();
        }
    }
}
