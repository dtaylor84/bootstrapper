using System.Collections.Generic;

namespace Bootstrap.Extensions.Containers
{
    public interface IBootstrapperContainerExtension: IBootstrapperExtension
    {
        object Container { get; }
        T Resolve<T>();
        IList<T> ResolveAll<T>();
        void Register<TTarget,TImplementation>() where TImplementation:TTarget;
        void Register<TTarget>(TTarget implementation);
        void RegisterAll<TTarget>();
        void SetServiceLocator();
        void ResetServiceLocator();
    }
}
