using SimpleInjector;

namespace YALV
{
    static class ServiceProvider
    {
        public static IServiceProvider Build()
        {
            var container = new Container();
            RegisterServices(container);
            var serviceProvider = container.RegisterServiceProvider();
            container.Verify();
            return serviceProvider;
        }

        private static void RegisterServices(Container container)
        {
            container.RegisterSingleton<ISelectedCultureAccessor>(() => new InMemoryCultureAccessor());
        }
    }
}
