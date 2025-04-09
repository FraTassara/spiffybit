using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpiffyBit.Localization
{
    [CreateAssetMenu(fileName = "Localization Settings", menuName = "Localization/Localization Settings")]
    public class LocalizationSettings : ScriptableObject
    {
        public string SpreadsheetID;
        public List<string> SpreadsheetNames = new List<string>();
        public List<TextAsset> Spreadsheets = new List<TextAsset>();
        public SystemLanguage DefaultLanguage = SystemLanguage.English;
        public List<LanguageCode> LanguageCodes = new List<LanguageCode>()
        {
            new LanguageCode(SystemLanguage.Arabic, "ar"),
            new LanguageCode(SystemLanguage.Bulgarian, "bg"),
            new LanguageCode(SystemLanguage.ChineseSimplified, "zh-CN"),
            new LanguageCode(SystemLanguage.ChineseTraditional, "zh-TW"),
            new LanguageCode(SystemLanguage.Czech, "cs"),
            new LanguageCode(SystemLanguage.Danish, "da"),
            new LanguageCode(SystemLanguage.Dutch, "nl"),
            new LanguageCode(SystemLanguage.English, "en"),
            new LanguageCode(SystemLanguage.Finnish, "fi"),
            new LanguageCode(SystemLanguage.French, "fr"),
            new LanguageCode(SystemLanguage.German, "de"),
            new LanguageCode(SystemLanguage.Greek, "el"),
            new LanguageCode(SystemLanguage.Hungarian, "hu"),
            new LanguageCode(SystemLanguage.Italian, "it"),
            new LanguageCode(SystemLanguage.Japanese, "ja"),
            new LanguageCode(SystemLanguage.Korean, "ko"),
            new LanguageCode(SystemLanguage.Norwegian, "no"),
            new LanguageCode(SystemLanguage.Polish, "pl"),
            new LanguageCode(SystemLanguage.Portuguese, "pt"),
            new LanguageCode(SystemLanguage.Romanian, "ro"),
            new LanguageCode(SystemLanguage.Russian, "ru"),
            new LanguageCode(SystemLanguage.Spanish, "es"),
            new LanguageCode(SystemLanguage.Swedish, "sv"),
            new LanguageCode(SystemLanguage.Thai, "th"),
            new LanguageCode(SystemLanguage.Turkish, "tr"),
            new LanguageCode(SystemLanguage.Ukrainian, "uk"),
            new LanguageCode(SystemLanguage.Vietnamese, "vn")
        };
    }

    [Serializable]
    public class LanguageCode
    {
        public SystemLanguage Language = SystemLanguage.English;
        public string Code;

        public LanguageCode() { }
        public LanguageCode(SystemLanguage systemLanguage, string code) => (Language, Code) = (systemLanguage, code);
    }
}