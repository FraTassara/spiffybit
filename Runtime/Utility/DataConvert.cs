using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpiffyBit.Localization.Utility
{
    internal static class DataConvert
    {
        public static SpreadsheetData ToSpreadsheetData(this List<Dictionary<string, string>> ob, string name)
        {
            var data = new Dictionary<string, SpreadsheetRow>();

            foreach (var dictionary in ob)
            {
                var languageRow = dictionary.ToSpreadsheetRow();
                if (!data.TryAdd(languageRow.Key, languageRow)) Debug.LogError($"Duplicated key: {languageRow.Key}");
            }

            return new SpreadsheetData()
            {
                SpreadsheetName = name,
                Data = data
            };
        }

        public static SpreadsheetRow ToSpreadsheetRow(this Dictionary<string, string> ob)
        {
            return new SpreadsheetRow()
            {
                Key = ob.Values.First(),
                LangTexts = new Dictionary<string, string>(ob)
            };
        }
    }
}