using Bootstrap.Tests.Extensions.TestImplementations;
using StructureMap.Configuration.DSL;

namespace Bootstrap.Tests.Extensions.Containers.StructureMap
{
    public class TestStructureMapRegistry : Registry
    {
        public TestStructureMapRegistry()
        {
            For<ITestInterface>().Use<TestImplementation>();
        }
    }
}