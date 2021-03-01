using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Common
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<TypeInfo> GetTypesThatImplementsInterface<TInterface>(params Assembly[] assembliesForScanning)
        {
            foreach(var assembly in assembliesForScanning)
            {
                foreach(var type in assembly.GetTypesThatImplementsInterface<TInterface>())
                {
                    yield return type;
                }
            }
        }

        public static IEnumerable<TypeInfo> GetTypesThatImplementsInterface<TInterface>(this Assembly assembly)
        {
            return assembly.DefinedTypes.Where(e => e.ImplementedInterfaces.Contains(typeof(TInterface)));
        }
    }
}
