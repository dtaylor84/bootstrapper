using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Bootstrap.WindsorExtension
{
    public class StartupTaskRegistration : IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            var containerExtension = Bootstrap.Bootstrapper.GetContainerExtension();
            if (containerExtension == null) return;

            containerExtension.LookForStartupTasks.AssemblyNames
                .ForEach(n => container.Register(AllTypes.Of<IStartupTask>().FromAssemblyNamed(n)));

            containerExtension.LookForStartupTasks.Assemblies
                .ForEach(a => container.Register(AllTypes.Of<IStartupTask>().FromAssembly(a)));

        }
    }
}
