using AutoMapper;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.AutoMapper
{
    public class AutoMapperRegistration: Profile, IBootstrapperRegistration
    {
        public void Register(IBootstrapperContainerExtension containerExtension)
        {
            containerExtension.Register<IProfileExpression>(Mapper.Configuration);
            containerExtension.Register(Mapper.Engine);
            containerExtension.RegisterAll<IMapCreator>();
            containerExtension.RegisterAll<Profile>();
        }
    }
}
