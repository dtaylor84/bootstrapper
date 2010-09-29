using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using AutoMapper;

namespace Bootstrapper.WindsorExtension
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
                ForEach(n => container.Register(AllTypes.Of<IMapCreator>().FromAssemblyNamed(n)));

            containerExtension.LookForMaps.Assemblies.
                ForEach(a => container.Register(AllTypes.Of<IMapCreator>().FromAssembly(a)));
        }

    }
}
