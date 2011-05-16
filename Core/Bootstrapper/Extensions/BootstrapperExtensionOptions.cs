namespace Bootstrap.Extensions
{
    public class BootstrapperExtensionOptions: IBootstrapperExtensionOptions
    {
        public BootstrapperExtensions And {get { return Bootstrapper.With; }}
        public void Start() { Bootstrapper.Start();}
    }
}
