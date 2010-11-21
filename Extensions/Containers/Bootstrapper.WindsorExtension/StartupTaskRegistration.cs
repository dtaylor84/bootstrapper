using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Bootstrap.WindsorExtension
{
    public class StartupTaskRegistration : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            var containerExtension = Bootstrapper.GetContainerExtension();
            if (containerExtension == null) return;

            containerExtension.LookForStartupTasks.AssemblyNames
                .ForEach(n => container.Register(AllTypes.FromAssemblyNamed(n).BasedOn<IStartupTask>()));

            containerExtension.LookForStartupTasks.Assemblies
                .ForEach(a => container.Register(AllTypes.FromAssembly(a).BasedOn<IStartupTask>()));

        }
    }
}
