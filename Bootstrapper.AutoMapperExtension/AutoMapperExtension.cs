using System;
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
            if (Bootstrapper.ContainerExtension != null && Bootstrapper.Container != null)
            {
                var containerExtension = Bootstrapper.ContainerExtension;
                var mapper = containerExtension.Resolve<IProfileExpression>();
                containerExtension.ResolveAll<IMapCreator>().ToList().ForEach(m => m.CreateMap(mapper));
            }
            else
            {
                RegistrationHelper.GetInstancesOfTypesImplementing<IMapCreator>()
                    .ForEach(m => m.CreateMap(Mapper.Configuration));
            }
        }

        public void Reset()
        {
            Mapper.Reset();  
        }
    }
}
