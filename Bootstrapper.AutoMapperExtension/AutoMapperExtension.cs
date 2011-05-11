using System.Linq;
using AutoMapper;
using Bootstrap.Extensions;

namespace Bootstrap.AutoMapper
{
    public class AutoMapperExtension : IBootstrapperExtension
    {
        public void Run()
        {
            var containerExtension = Bootstrapper.ContainerExtension;
            var mapper = containerExtension.Resolve<IProfileExpression>();
            containerExtension.ResolveAll<IMapCreator>().ToList().ForEach(m => m.CreateMap(mapper));
        }

        public void Reset()
        {
            Mapper.Reset();  
        }
    }
}
