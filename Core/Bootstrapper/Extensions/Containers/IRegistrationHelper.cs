using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bootstrap.Extensions.Containers
{
    public interface IRegistrationHelper
    {
        IEnumerable<Type> GetTypesImplementing<T>();
        IEnumerable<Type> GetTypesImplementing<T>(string assemblyName);
        IEnumerable<Type> GetTypesImplementing<T>(Assembly assembly);
        IEnumerable<Assembly> GetAssemblies();
        List<T> GetInstancesOfTypesImplementing<T>();
    }
}