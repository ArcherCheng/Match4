using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace MatchApi.Helper
{
    public class Ioc
    {
        private static readonly UnityContainer _container;
        static Ioc()
        {
            _container = new UnityContainer();
        }

        public static void RegisterInheritedTypes(System.Reflection.Assembly assembly, Type baseType)
        {
            _container.RegisterInheritedTypes(assembly, baseType);
        }

        public static void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _container.RegisterType<TInterface, TImplementation>();
        }

        public static T Get<T>()
        {
            return _container.Resolve<T>();
        }

    }
    public static class UnityContainerExtensions
    {
        public static void RegisterInheritedTypes(this Unity.IUnityContainer container, System.Reflection.Assembly assembly, System.Type baseType)
        {
            var allTypes = assembly.GetTypes();
            var baseInterfaces = baseType.GetInterfaces();

            foreach (var type in allTypes)
            {
                var test = type.BaseType;
                if (type.BaseType != null && type.BaseType.GenericEq(baseType))
                {
                    var typeInterface = type.GetInterfaces().FirstOrDefault(x => !baseInterfaces.Any(bi => bi.GenericEq(x)));
                    if (typeInterface == null)
                    {
                        continue;
                    }
                    container.RegisterType(typeInterface, type);
                    //container.RegisterSingleton(typeInterface, type);
                }
            }

            var ok = assembly.GetTypes();
        }
    }

    public static class TypeExtensions
    {
        public static bool GenericEq(this Type type, Type toCompare)
        {
            return type.Namespace == toCompare.Namespace && type.Name == toCompare.Name;
        }

    }
}
