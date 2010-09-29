namespace Bootstrapper.StructureMapExtension
{
    public interface IStructureMapRegistration
    {
        void Register(StructureMap.IContainer container);
    }
}
