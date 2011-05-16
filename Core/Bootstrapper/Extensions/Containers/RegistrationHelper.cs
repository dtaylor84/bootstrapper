using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bootstrap.Extensions.Containers
{
    public class RegistrationHelper
    {

        public static IEnumerable<Type> GetTypesImplementing<T>()
        {
            return GetTypesImplementing<T>(Assembly.GetCallingAssembly());
        }

        public static IEnumerable<Type> GetTypesImplementing<T>(string assemblyName)
        {
            return GetTypesImplementing<T>(Assembly.Load(assemblyName));
        }

        public static IEnumerable<Type> GetTypesImplementing<T>(Assembly assembly)
        {
            return  assembly.GetExportedTypes().Where(t =>  t.IsPublic &&
                                                    !t.IsAbstract &&
                                                    typeof (T).IsAssignableFrom(t));
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic &&
                                                                      IsNotExcluded(a));
        }

        private static bool IsNotExcluded(Assembly assembly)
        {
            return !Bootstrapper.Excluding.Assemblies.Any(e => assembly.FullName.StartsWith(e));
        }
    }
}
