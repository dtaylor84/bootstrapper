using Microsoft.Practices.ServiceLocation;
using StructureMap;
using StructureMap.ServiceLocatorAdapter;

namespace Bootstrapper.StructureMapExtension
{
    public class StructureMapContainerExtension: BootstrapperContainerExtension
    {

        public IContainer Container { get; set; }

        protected override void InitializeContainer()
        {
            Container = new Container();
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            Container.Configure(c => c.Scan(s =>
                                                {
                                                    s.AddAllTypesOf<IStructureMapRegistration>();
                                                    foreach(var assemblyName in LookForRegistrations.AssemblyNames)
                                                        s.Assembly(assemblyName);
                                                    foreach (var assembly in LookForRegistrations.Assemblies)
                                                        s.Assembly(assembly);
                                                }));
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            foreach(var registration in Container.GetAllInstances<IStructureMapRegistration>())
                registration.Register(Container);
        }

        protected override void InitializeServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(Container));
        }

        protected override void ResetContainer()
        {
            Container = null;
            SetContainer(Container);
        }
    }
}
