using StructureMap;

namespace Bootstrap.StructureMap
{
    public interface IStructureMapRegistration
    {
        void Register(IContainer container);
    }
}
