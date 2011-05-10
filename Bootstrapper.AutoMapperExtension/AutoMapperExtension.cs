using System.Linq;
using AutoMapper;
using Bootstrap.Extensions;

namespace Bootstrap.AutoMapper
{
    public class AutoMapperExtension : IBootstrapperExtension
    {
        public void Run()
        {
            var container = Bootstrapper.ContainerExtension;
            var mapper = container.Resolve<IProfileExpression>();
            container.ResolveAll<IMapCreator>().ToList().ForEach(m => m.CreateMap(mapper));
        }

        public void Reset()
        {
            Mapper.Reset();               
        }
    }
}
