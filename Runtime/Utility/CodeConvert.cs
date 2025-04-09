using UnityEngine;
using System.Linq;

namespace SpiffyBit.Localization.Utility
{
    public static class CodeConvert
    {
        public static LanguageCode ToLanguageCode(this SystemLanguage systemLanguage)
        {
            var localizationSettings = LocalizationCore.GetSettings();
            return localizationSettings.LanguageCodes.FirstOrDefault(x => x.Language == systemLanguage);
        }

        public static LanguageCode ToLanguageCode(this SystemLanguage systemLanguage, LocalizationSettings localizationSettings)
        {
            return localizationSettings.LanguageCodes.FirstOrDefault(x => x.Language == systemLanguage);
        }
    }
}