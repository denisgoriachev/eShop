using eShop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Infrastructure
{
    public static class EventTypeMap
    {
        private static readonly Dictionary<string, Type> _reverseMap = new();
        private static readonly Dictionary<Type, string> _map = new();

        public static string GetTypeName<T>() where T: IDomainEvent => _map[typeof(T)];

        public static string GetTypeName(object o) => _map[o.GetType()];

        public static Type GetType(string typeName) => _reverseMap[typeName];

        public static void AddType<T>(string name) where T : IDomainEvent => AddType(typeof(T), name);

        public static void AddType(Type type, string name) 
        {
            _reverseMap[name] = type;
            _map[type] = name;
        }
    }
}
