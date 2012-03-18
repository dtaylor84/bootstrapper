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
            IProfileExpression configuration;

            if (Bootstrapper.ContainerExtension != null && Bootstrapper.Container != null)
            {
                configuration = Bootstrapper.ContainerExtension.Resolve<IProfileExpression>();
                mapCreators = Bootstrapper.ContainerExtension.ResolveAll<IMapCreator>().ToList();
            }
            else
            {
                configuration = Mapper.Configuration;
                mapCreators = registrationHelper.GetInstancesOfTypesImplementing<IMapCreator>();
            }
            mapCreators.ForEach(m => m.CreateMap(configuration));            
        }

        public void Reset()
        {
            Mapper.Reset();  
        }
    }
}
