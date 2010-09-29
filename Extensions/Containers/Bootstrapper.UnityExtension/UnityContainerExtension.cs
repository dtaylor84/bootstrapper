using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Bootstrapper.UnityExtension
{
    public class UnityContainerExtension: BootstrapperContainerExtension
    {
        public IUnityContainer Container { get; private set; }

        protected override void InitializeContainer()
        {
            Container = new UnityContainer();
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            LookForRegistrations.AssemblyNames.
                ForEach(n => RegistrationHelper.GetTypesImplementing<IUnityRegistration>(n).
                    ForEach(t => Container.RegisterType(typeof(IUnityRegistration), t, t.Name)));

            LookForRegistrations.Assemblies.
                ForEach(a => RegistrationHelper.GetTypesImplementing<IUnityRegistration>(a).
                    ForEach(t => Container.RegisterType(typeof(IUnityRegistration), t, t.Name)));
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            Container.ResolveAll<IUnityRegistration>().ForEach(r => r.Register(Container));
        }

        protected override void InitializeServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(Container));
            SetContainer(Container);
        }

        protected override void ResetContainer()
        {
            Container = null;
            SetContainer(Container);
        }
    }
}
