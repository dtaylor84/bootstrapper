using System;
using System.Collections.Generic;

namespace Bootstrap.Extensions.Containers
{
    public interface IBootstrapperContainerExtension: IBootstrapperExtension
    {
        object Container { get; }
        T Resolve<T>() where T:class;
        IList<T> ResolveAll<T>() where T:class;
        void RegisterAll(Type target);
        void Register<TTarget,TImplementation>() where TTarget:class where TImplementation:class, TTarget;
        void Register<TTarget>(TTarget implementation) where TTarget : class;
        void RegisterAll<TTarget>() where TTarget : class;
        void SetServiceLocator();
        void ResetServiceLocator();
    }
}
