namespace Bootstrap.Extensions.Containers
{
// ReSharper disable UnusedTypeParameter
    public interface IRegistrationInvoker<TTarget,TImplementation> where TImplementation:TTarget
// ReSharper restore UnusedTypeParameter
    {
        void Register();
    }
}
