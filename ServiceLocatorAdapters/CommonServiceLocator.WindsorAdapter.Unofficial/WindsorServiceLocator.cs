using System;
using System.Collections.Generic;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;

namespace CommonServiceLocator.WindsorAdapter.Unofficial
{
	public class WindsorServiceLocator : ServiceLocatorImplBase
	{
		private readonly IWindsorContainer container;

		public WindsorServiceLocator(IWindsorContainer container) {this.container = container;}

		protected override object DoGetInstance(Type serviceType, string key)
		{
            return key!=null ? container.Resolve(key, serviceType) : container.Resolve(serviceType);
		}

		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			return (object[])container.ResolveAll(serviceType);
		}
	}
}
