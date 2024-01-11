using System.Globalization;

namespace YALV
{
    class InMemoryCultureAccessor : ISelectedCultureAccessor
    {
        private CultureInfo culture = CultureInfo.GetCultureInfo("en-US");

        public CultureInfo GetCulture()
        {
            return culture;
        }

        public void SetCulture(CultureInfo culture)
        {
            this.culture = culture;
        }
    }
}
