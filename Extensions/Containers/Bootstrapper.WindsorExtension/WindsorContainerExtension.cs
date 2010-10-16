using Castle.Facilities.FactorySupport;
using Castle.Windsor;
using Castle.Core;
using Castle.MicroKernel.Registration;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;

namespace Bootstrap.WindsorExtension
{
    public class WindsorContainerExtension: BootstrapperContainerExtension
    {
        public IWindsorContainer Container { get; private set; }

        protected override void InitializeContainer()
        {
            Container = new WindsorContainer()
                .AddFacility<FactorySupportFacility>();
        }

        protected override void RegisterImplementationsOfIRegistration()
        {
            LookForRegistrations.AssemblyNames
                .ForEach(n => Container.Register(AllTypes.Of<IWindsorRegistration>().FromAssemblyNamed(n)));

            LookForRegistrations.Assemblies
                .ForEach(a => Container.Register(AllTypes.Of<IWindsorRegistration>().FromAssembly(a)));
        }

        protected override void InvokeRegisterForImplementationsOfIRegistration()
        {
            Container.ResolveAll<IWindsorRegistration>().ForEach(r => r.Register(Container));
        }

        protected override void InitializeServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(Container));
            SetContainer(Container);
        }

        protected override void ResetContainer()
        {
            Container = null;
            SetContainer(Container);
        }



    }
}
