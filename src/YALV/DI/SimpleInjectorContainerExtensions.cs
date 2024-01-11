using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjector;

namespace YALV
{
    internal static class SimpleInjectorContainerExtensions
    {
        internal class ServiceProvider : IServiceProvider
        {
            private readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertiesCache = new ConcurrentDictionary<Type, PropertyInfo[]>();
            private readonly Container _container;

            public ServiceProvider(Container container)
            {
                _container = container;
            }

            object System.IServiceProvider.GetService(Type serviceType)
            {
                var obj = _container.GetInstance(serviceType);
                InjectProperties(obj);
                return obj;
            }

            T IServiceProvider.GetService<T>()
            {
                var obj = _container.GetInstance<T>();
                InjectProperties(obj);
                return obj;
            }

            private void InjectProperties(object instance)
            {
                var properties = _propertiesCache.GetOrAdd(instance.GetType(), t => GetPropertiesToInject(t).ToArray());
                foreach (var property in properties) {
                    var obj = _container.GetInstance(property.PropertyType);
                    property.SetValue(instance, obj);
                }
            }

            private static IEnumerable<PropertyInfo> GetPropertiesToInject(Type type)
            {
                var instanceProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                return instanceProperties.Where(p => p.CanWrite && Attribute.IsDefined(p, typeof(InjectAttribute)));
            }
        }

        public static IServiceProvider RegisterServiceProvider(this Container container)
        {
            var provider = new ServiceProvider(container);
            container.RegisterInstance<IServiceProvider>(provider);
            return provider;
        }
    }
}
