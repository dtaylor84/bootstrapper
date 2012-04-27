using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Tests.Extensions.TestImplementations;

namespace Bootstrap.Tests.Extensions.AutoMapper
{
    public class TestAutoMapperMapCreator: IMapCreator
    {
        public void CreateMap(IProfileExpression mapper)
        {
            mapper.CreateMap<TestImplementation, AnotherTestImplementation>();
        }
    }
}
