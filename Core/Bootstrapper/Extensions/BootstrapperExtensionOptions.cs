namespace Bootstrap.Extensions
{
    public class BootstrapperExtensionOptions: IBootstrapperExtensionOptions
    {
        public BootstrapperExtensions And {get { return Bootstrapper.With; }}
    }
}
