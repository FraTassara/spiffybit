using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpiffyBit.Localization.Utility
{
    public static class DataUtility
    {
        public static string GetAllCharacters(LanguageCode languageCode)
        {
            if (!Application.isEditor) return null;
            LocalizationCore.UpdateData();

            string langData = "";
            foreach (var spreadsheet in LocalizationCore._data)
            {
                foreach (var row in spreadsheet.Value.Data)
                {
                    bool success = row.Value.LangTexts.TryGetValue(languageCode.Code, out var result);
                    if (success) langData += result;
                }
            }

            //Add Lower and upper characters
            string content = new string(langData.Distinct().ToArray());
            string allCharacters = $"{content.ToLower()}{content.ToUpper()}";

            //Remove duplicated characters for asian languages
            return new string(allCharacters.Distinct().ToArray());
        }
    }
}