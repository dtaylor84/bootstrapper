﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bootstrap.Extensions.Containers
{
    public class RegistrationHelper : IRegistrationHelper
    {
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
            return assembly.GetExportedTypes().Where(t => t.IsPublic &&
                                                          !t.IsAbstract &&
                                                          typeof (T).IsAssignableFrom(t));
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return Bootstrapper.IncludingOnly.Assemblies.Any() 
                    ? Bootstrapper.IncludingOnly.Assemblies
                    :AppDomain.CurrentDomain.GetAssemblies()
                        .Where(a => !a.IsDynamic && IsNotExcluded(a));
        }

        private static bool IsNotExcluded(Assembly assembly)
        {
            return  Bootstrapper.Including.Assemblies.Any(e => assembly.FullName == e.FullName) || 
                    !Bootstrapper.Excluding.Assemblies.Any(e => assembly.FullName.StartsWith(e));
        }

        public List<T> GetInstancesOfTypesImplementing<T>()
        {
            var instances = new List<T>();
            GetAssemblies().ToList()
                .ForEach(a => GetTypesImplementing<T>(a).ToList()
                    .ForEach(t => instances.Add((T)Activator.CreateInstance(t))));
            return instances;
        }
    }
}