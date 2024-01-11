namespace YALV
{
    internal interface IServiceProvider : System.IServiceProvider
    {
        T GetService<T>() where T : class;
    }
}
