using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using AutoMapper;

namespace Bootstrap.WindsorExtension
{
    public class AutoMapperRegistration: IWindsorRegistration
    {
        public void Register(IWindsorContainer container)
        {
            RegisterMappingEngine(container);
            RegisterMapCreators(container);
        }

        private static void RegisterMappingEngine(IWindsorContainer container)
        {
            container.Register(Component.For<IMappingEngine>()
                                   .UsingFactoryMethod(() => Mapper.Engine));
        }

        private static void RegisterMapCreators(IWindsorContainer container)
        {
            var containerExtension = Bootstrapper.GetContainerExtension();
            if (containerExtension == null) return;

            containerExtension.LookForMaps.AssemblyNames.
                ForEach(n => container.Register(AllTypes.FromAssemblyNamed(n).BasedOn<IMapCreator>()));

            containerExtension.LookForMaps.Assemblies.
                ForEach(a => container.Register(AllTypes.FromAssembly(a).BasedOn<IMapCreator>()));
        }

    }
}
