using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.Tests.Extensions.TestImplementations
{
    public class TestMapCreator: IMapCreator
    {
        public void CreateMap(IProfileExpression mapper)
        {
            mapper.CreateMap<BootstrapperContainerExtension, IBootstrapperExtension>();
        }
    }
}
