using System;
using SimpleInjector;

namespace YALV
{
    static class ServiceProvider
    {
        public static IServiceProvider Build(Action<Container> configRoot)
        {
            var container = new Container();
            configRoot(container);
            var serviceProvider = container.RegisterServiceProvider();
            container.Verify();
            return serviceProvider;
        }
    }
}
