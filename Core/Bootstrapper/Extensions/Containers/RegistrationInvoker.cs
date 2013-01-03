namespace Bootstrap.Extensions.Containers
{
    public class RegistrationInvoker<TTarget,TImplementation>: IRegistrationInvoker<TTarget,TImplementation> where TTarget:class where TImplementation:class, TTarget
    {
        private readonly IBootstrapperContainerExtension containerExtension;

        public RegistrationInvoker(IBootstrapperContainerExtension containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        public void Register()
        {
            containerExtension.Register<TTarget,TImplementation>();
        }
    }
}
