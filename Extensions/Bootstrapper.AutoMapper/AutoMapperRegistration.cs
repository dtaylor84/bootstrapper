using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bootstrap.Extensions.Containers;

namespace Bootstrap.AutoMapper
{
    public class AutoMapperRegistration: IBootstrapperRegistration
    {
        public void Register(IBootstrapperContainerExtension containerExtension)
        {
            containerExtension.Register(AutoMapperExtension.ProfileExpression);
            containerExtension.Register(AutoMapperExtension.ConfigurationProvider);
            containerExtension.Register(AutoMapperExtension.Mapper);
            containerExtension.Register(AutoMapperExtension.Engine);
            containerExtension.Register<IExpressionBuilder, ExpressionBuilder>();
            containerExtension.RegisterAll<IMapCreator>();
            containerExtension.RegisterAll<Profile>();
        }
    }
}
