namespace Bootstrap.Tests.Extensions.TestImplementations
{
    public class GenericTest<T> : IGenericTest<T> where T : class
    {
        public T Default()  {return default(T);}
    }
}