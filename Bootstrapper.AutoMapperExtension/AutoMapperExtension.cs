using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.AutoMapper
{
    public class AutoMapperExtension : IBootstrapperExtension
    {
        private readonly IRegistrationHelper registrationHelper;

        public AutoMapperExtension(IRegistrationHelper registrationHelper)
        {
            Bootstrapper.Excluding.Assembly("AutoMapper");
            this.registrationHelper = registrationHelper;
        }

        public void Run()
        {
            List<IMapCreator> mapCreators;
            List<Profile> profiles;

            if (Bootstrapper.ContainerExtension != null && Bootstrapper.Container != null)
            {
                mapCreators = Bootstrapper.ContainerExtension.ResolveAll<IMapCreator>().ToList();
                profiles = Bootstrapper.ContainerExtension.ResolveAll<Profile>().ToList();
            }
            else
            {
                mapCreators = registrationHelper.GetInstancesOfTypesImplementing<IMapCreator>();
                profiles = registrationHelper.GetInstancesOfTypesImplementing<Profile>();
            }
            Mapper.Initialize(c =>
                                  {
                                      profiles.ForEach(c.AddProfile);
                                      mapCreators.ForEach(m => m.CreateMap(c));
                                  });
        }

        public void Reset()
        {
            Mapper.Reset();  
        }
    }
}
