using AutoMapper;
using StructureMap;

namespace Bootstrap.StructureMapExtension
{
    public class AutoMapperRegistration:IStructureMapRegistration
    {
        public void Register(IContainer container)
        {
            RegisterMappingEngine(container);
            RegisterMapCreators(container);
        }

        private static void RegisterMappingEngine(IContainer container)
        {
            container.Configure(c => c.For<IMappingEngine>().Use(Mapper.Engine));
        }

        private static void RegisterMapCreators(IContainer container)
        {
            var containerExtension = Bootstrap.Bootstrapper.GetContainerExtension();
            if (containerExtension == null) return;

            container.Configure(c => c.Scan(s =>
                                                {
                                                    s.AddAllTypesOf<IMapCreator>();
                                                    foreach (var assemblyName in containerExtension.LookForMaps.AssemblyNames)
                                                        s.Assembly(assemblyName);
                                                    foreach (var assembly in containerExtension.LookForMaps.Assemblies)
                                                        s.Assembly(assembly);

                                                }));
        }
    }
}
