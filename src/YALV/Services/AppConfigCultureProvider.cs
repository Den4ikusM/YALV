using System;
using System.Configuration;
using System.Globalization;

namespace YALV
{
    class AppConfigCultureProvider : IUiCultureProvider
    {
        private static readonly CultureInfo DefaultCulture = CultureInfo.GetCultureInfo("en-US");

        private static readonly Lazy<CultureInfo> SelectedCulture = new Lazy<CultureInfo>(() =>
        {
            CultureInfo culture = null;
            try
            {
                var cultureName = ConfigurationManager.AppSettings["Culture"];
                if (!string.IsNullOrWhiteSpace(cultureName))
                    culture = CultureInfo.GetCultureInfo(cultureName);
            }
            catch (CultureNotFoundException ex)
            {
            }
            return culture;
        });

        public CultureInfo GetCulture()
        {
            return SelectedCulture.Value ?? DefaultCulture;
        }
    }
}