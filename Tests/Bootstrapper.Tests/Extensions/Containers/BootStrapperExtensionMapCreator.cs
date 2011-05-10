using AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Tests.Extensions.Containers
{
    public class BootStrapperExtensionMapCreator: IMapCreator
    {
        public void CreateMap(IProfileExpression mapper)
        {
            mapper.CreateMap<IBootstrapperExtension, BootstrapperContainerExtension>();
        }
    }
}
