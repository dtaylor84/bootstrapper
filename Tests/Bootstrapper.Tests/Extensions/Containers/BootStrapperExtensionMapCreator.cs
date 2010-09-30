using AutoMapper;

namespace Bootstrap.Tests.Extensions.Containers
{
    public class BootStrapperExtensionMapCreator: IMapCreator
    {
        public void CreateMap()
        {
            Mapper.CreateMap<BootstrapperExtension, BootstrapperExtension>();
        }
    }
}
