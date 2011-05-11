using System;
using System.Collections.Generic;
using System.Linq;
using Bootstrap.Extensions.Containers;
using StructureMap;
using StructureMap.ServiceLocatorAdapter;
using Microsoft.Practices.ServiceLocation;

namespace Bootstrap.StructureMap
{
    public class StructureMapExtension : BootstrapperContainerExtension
    {
        private IContainer container;

        public void InitializeContainer(IContainer aContainer)
        {
            container = aContainer;
            Container = container;
        }

        protected override void InitializeContainer()
        {
            container = new Container();
            Container = container;
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            RegisterAll<IBootstrapperRegistration>();
            RegisterAll<IStructureMapRegistration>();
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            container.GetAllInstances<IBootstrapperRegistration>().ToList().ForEach(r => r.Register(this));
            container.GetAllInstances<IStructureMapRegistration>().ToList().ForEach(r => r.Register(container));
        }

        public override void SetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));
        }

        protected override void ResetContainer()
        {
            container = null;
            Container = null;
        }

        public override void ResetServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => null);
        }

        public override T Resolve<T>()
        {
            return container.GetInstance<T>();
        }

        public override IList<T> ResolveAll<T>()
        {
            return container.GetAllInstances<T>();
        }

        public override void Register<TTarget, TImplementation>()
        {
            container.Configure(c => c.For<TTarget>().Use<TImplementation>());
        }

        public override void Register<TTarget>(TTarget implementation)
        {
            container.Configure(c => c.For<TTarget>().Use(implementation));
        }

        public override void RegisterAll<TTarget>()
        {
            container.Configure(c =>
                                c.Scan(s =>
                                           {
                                               s.AddAllTypesOf<TTarget>();
                                               foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.IsDynamic))
                                                   s.Assembly(assembly);
                                           }));
        }
    }
}
