using AutoMapper;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.AutoMapper
{
    public class AutoMapperRegistration: IBootstrapperRegistration
    {
        public void Register(IBootstrapperContainerExtension container)
        {
            container.Register<IProfileExpression>(Mapper.Configuration);
            container.Register(Mapper.Engine);
            container.RegisterAll<IMapCreator>();
        }
    }
}
