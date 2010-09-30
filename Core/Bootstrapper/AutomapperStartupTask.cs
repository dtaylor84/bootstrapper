using Microsoft.Practices.ServiceLocation;
using AutoMapper;

namespace Bootstrap
{
    public class AutoMapperStartupTask: IStartupTask
    {
        public void Run()
        {
            foreach(var mapCreator in ServiceLocator.Current.GetAllInstances<IMapCreator>())
                mapCreator.CreateMap();        
        }

        public void Reset()
        {
            Mapper.Reset();
        }
    }
}
