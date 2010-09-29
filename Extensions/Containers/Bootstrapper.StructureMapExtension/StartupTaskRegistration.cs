using StructureMap;

namespace Bootstrapper.StructureMapExtension
{
    public class StartupTaskRegistration: IStructureMapRegistration
    {
        public void Register(IContainer container)
        {
            var containerExtension = Bootstrapper.GetContainerExtension();
            if (containerExtension == null) return;

            container.Configure(c => c.Scan(s =>
            {
                s.AddAllTypesOf<IStartupTask>();
                foreach (var assemblyName in containerExtension.LookForStartupTasks.AssemblyNames)
                    s.Assembly(assemblyName);
                foreach (var assembly in containerExtension.LookForStartupTasks.Assemblies)
                    s.Assembly(assembly);

            }));
        }
    }
}
