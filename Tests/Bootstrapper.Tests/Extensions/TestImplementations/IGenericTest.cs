namespace Bootstrap.Tests.Extensions.TestImplementations
{
    public interface IGenericTest<out T> where T:class
    {
        T Default();
    }
}