namespace Bootstrap
{
    public interface IAssemblySetOptions
    {
        IBootstrapperOption LoadedAssemblies();
        IBootstrapperOption ReferencedAssemblies();
    }
}