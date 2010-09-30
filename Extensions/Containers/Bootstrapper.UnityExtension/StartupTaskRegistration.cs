using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Bootstrap.UnityExtension
{
    public class StartupTaskRegistration : IUnityRegistration
    {
        public void Register(IUnityContainer container)
        {
            var containerExtension = Bootstrap.Bootstrapper.GetContainerExtension();
            if (containerExtension == null) return;

            containerExtension.LookForStartupTasks.AssemblyNames.
                ForEach(n => RegistrationHelper.GetTypesImplementing<IStartupTask>(n).
                    ForEach(t => container.RegisterType(typeof(IStartupTask), t, t.Name)));

            containerExtension.LookForStartupTasks.Assemblies.
                ForEach(a => RegistrationHelper.GetTypesImplementing<IStartupTask>(a).
                    ForEach(t => container.RegisterType(typeof(IStartupTask), t, t.Name)));
        }
    }
}
