using Bootstrap.Extensions.Containers;

namespace Bootstrap
{
    public class AssemblySetOptions:BootstrapperOption, IAssemblySetOptions
    {
        public IBootstrapperOption LoadedAssemblies()
        {
            Bootstrapper.RegistrationHelper = new RegistrationHelper(new LoadedAssemblyProvider());
            return this;
        }

        public IBootstrapperOption ReferencedAssemblies()
        {
            Bootstrapper.RegistrationHelper = new RegistrationHelper(new ReferencedAssemblyProvider());
            return this;
        }
    }
}