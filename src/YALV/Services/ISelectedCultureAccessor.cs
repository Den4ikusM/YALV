using System.Globalization;

namespace YALV
{
    interface ISelectedCultureAccessor
    {
        CultureInfo GetCulture();
        void SetCulture(CultureInfo culture);
    }
}
