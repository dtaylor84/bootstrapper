using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bootstrap.Extensions.Containers
{
    public class RegistrationHelper : IRegistrationHelper
    {
        internal readonly IBootstrapperAssemblyProvider AssemblyProvider;

        public RegistrationHelper(IBootstrapperAssemblyProvider assemblyProvider)
        {
            AssemblyProvider = assemblyProvider;
        }

        public IEnumerable<Type> GetTypesImplementing(Type t)
        {
            return 
                GetTypesFromAssemblyImplementing(Assembly.GetCallingAssembly(), t);
        }

        public IEnumerable<Type> GetTypesImplementing(string assemblyName, Type t)
        {
            return GetTypesFromAssemblyImplementing(Assembly.Load(assemblyName), t);
        }

        public IEnumerable<Type> GetTypesImplementing(Assembly assembly, Type t)
        {
            return GetTypesFromAssemblyImplementing(assembly, t);
        }

        public IEnumerable<Type> GetTypesImplementing<T>()
        {
            return GetTypesImplementing<T>(Assembly.GetCallingAssembly());
        }

        public IEnumerable<Type> GetTypesImplementing<T>(string assemblyName)
        {
            return GetTypesImplementing<T>(Assembly.Load(assemblyName));
        }

        public IEnumerable<Type> GetTypesImplementing<T>(Assembly assembly)
        {
            return GetTypesFromAssemblyImplementing(assembly, typeof (T));
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return Bootstrapper.IncludingOnly.Assemblies.Any() 
                    ? Bootstrapper.IncludingOnly.Assemblies
                    :AssemblyProvider.GetAssemblies()
                        .Where(a => !a.IsDynamic && IsNotExcluded(a));
        }

        public List<T> GetInstancesOfTypesImplementing<T>()
        {
            var instances = new List<T>();
            GetAssemblies().ToList()
                .ForEach(a => GetTypesImplementing<T>(a).ToList()
                    .ForEach(t => instances.Add((T)Activator.CreateInstance(t))));
            return instances;
        }

        private static bool IsNotExcluded(Assembly assembly)
        {
            return  Bootstrapper.Including.Assemblies.Any(e => assembly.FullName == e.FullName) || 
                    !Bootstrapper.Excluding.Assemblies.Any(e => assembly.FullName.StartsWith(e));
        }

        private static IEnumerable<Type> GetTypesFromAssemblyImplementing(Assembly assembly, Type type)
        {
            return assembly.GetExportedTypes().Where(t => t.IsPublic &&
                                                          !t.IsAbstract &&
                                                          IsAssignableTo(t, type));
        }

        private static bool IsAssignableTo(Type aType, Type anotherType)
        {
            return anotherType.IsGenericType
                ? IsAssignableToGenericType(aType, anotherType)
                : anotherType.IsAssignableFrom(aType);
        }

        private static bool IsAssignableToGenericType(Type aType, Type genericType)
        {
            return aType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType)
                   || (aType.IsGenericType && aType.GetGenericTypeDefinition() == genericType)
                   || (aType.BaseType != null && IsAssignableToGenericType(aType.BaseType, genericType));
        }
        
    }
}