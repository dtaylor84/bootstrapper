using AutoMapper;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Bootstrapper.UnityExtension
{
    public class AutoMapperRegistration: IUnityRegistration
    {
        public void Register(IUnityContainer container)
        {
            RegisterMappingEngine(container);
            RegisterMapCreators(container);
        }

        private static void RegisterMappingEngine(IUnityContainer container)
        {
            container.RegisterInstance(Mapper.Engine);
        }

        private static void RegisterMapCreators(IUnityContainer container)
        {
            var containerExtension = Bootstrapper.GetContainerExtension();
            if (containerExtension == null) return;

            containerExtension.LookForMaps.AssemblyNames.
                ForEach(n => RegistrationHelper.GetTypesImplementing<IMapCreator>(n).
                    ForEach(t => container.RegisterType(typeof(IMapCreator), t, t.Name)));
                    
            containerExtension.LookForMaps.Assemblies.
                ForEach(a => RegistrationHelper.GetTypesImplementing<IMapCreator>(a).
                    ForEach(t => container.RegisterType(typeof(IMapCreator), t, t.Name)));
        }
    }
}
