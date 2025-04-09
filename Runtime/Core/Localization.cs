using SpiffyBit.Localization.Utility;
using UnityEngine;
using System;

namespace SpiffyBit.Localization
{
    public static class Localization
    {
        public static SystemLanguage Language
        {
            get => LocalizationCore.LanguageCode.Language;
            set
            {
                LocalizationCore.LanguageCode = value.ToLanguageCode();
                OnLanguageChanged?.Invoke();
            }
        }

        public static event Action OnLanguageChanged;

        public static string Get(string key, string alternativeText = null, string spreadsheet = null)
        {
            string data = string.IsNullOrEmpty(spreadsheet) ? LocalizationCore.GetData(key) : LocalizationCore.GetData(spreadsheet, key);
            return string.IsNullOrEmpty(data) ? alternativeText : data;
        }
    }
}