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
        IEnumerable<Type> GetTypesImplementing(Type t);
        IEnumerable<Type> GetTypesImplementing(string assemblyName, Type t);
        IEnumerable<Type> GetTypesImplementing(Assembly assembly, Type t);
    }
}