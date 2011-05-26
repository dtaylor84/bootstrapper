using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.AutoMapper
{
    public class AutoMapperExtension : IBootstrapperExtension
    {
        public AutoMapperExtension()
        {
            Bootstrapper.Excluding.Assembly("AutoMapper");            
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
                mapCreators = RegistrationHelper.GetInstancesOfTypesImplementing<IMapCreator>();
            }
            mapCreators.ForEach(m => m.CreateMap(configuration));            
        }

        public void Reset()
        {
            Mapper.Reset();  
        }
    }
}
