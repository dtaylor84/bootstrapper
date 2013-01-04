namespace Bootstrap.Extensions.Containers
{
// ReSharper disable UnusedTypeParameter
    public interface IRegistrationInvoker<TTarget,TImplementation> where TTarget : class where TImplementation : class, TTarget
// ReSharper restore UnusedTypeParameter
    {
        void Register();
    }
}
