using AutoMapper;

namespace Bootstrap.AutoMapper
{
    public interface IMapCreator
    {
        void CreateMap(IProfileExpression mapper);
    }
}
