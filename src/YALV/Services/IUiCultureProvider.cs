using System.Globalization;

namespace YALV
{
    public interface IUiCultureProvider
    {
        CultureInfo GetCulture();
    }
}