using SpiffyBit.Localization.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace SpiffyBit.Localization
{
    internal class LocalizationCore
    {
        internal static LanguageCode LanguageCode;

        internal static Dictionary<string, SpreadsheetData> _data = new Dictionary<string, SpreadsheetData>();

        #region Load Data

        static LocalizationCore()
        {
            var settings = GetSettings();                                             // Load Settings File
            settings.Spreadsheets.ForEach(AddSpreadsheet);                            // Load All Json Files From Settings List
            LanguageCode = settings.DefaultLanguage.ToLanguageCode(settings);         // Set Default Language
        }

        internal static LocalizationSettings GetSettings()
        {
            var localizationSettings = Resources.LoadAll<LocalizationSettings>("");
            return localizationSettings.Length > 0 ? localizationSettings[0] : ScriptableObject.CreateInstance<LocalizationSettings>();
        }

        private static void AddSpreadsheet(TextAsset textAsset)
        {
            try
            {
                if (textAsset == null) return;
                var spreadsheet = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(textAsset.text);
                SpreadsheetData spreadsheetData = spreadsheet.ToSpreadsheetData(textAsset.name);
                if (!_data.TryAdd(spreadsheetData.SpreadsheetName, spreadsheetData)) Debug.LogError($"Duplicated file: {spreadsheetData.SpreadsheetName}!");
            }
            catch { Debug.LogError($"Invalid content in file: {textAsset.name}"); }
        }

        internal static void UpdateData()
        {
            _data.Clear();
            var settings = GetSettings();
            settings.Spreadsheets.ForEach(AddSpreadsheet);
        }

        #endregion

        #region Get Data

        internal static string GetData(string spreadsheet, string key)
        {
            if (!_data.TryGetValue(spreadsheet, out var spreadsheetData)) return null;
            if (!spreadsheetData.Data.TryGetValue(key, out var spreadsheetRow)) return null;
            if (!spreadsheetRow.LangTexts.TryGetValue(LanguageCode.Code, out var resultText)) return null;

            return resultText;
        }

        internal static string GetData(string key)
        {
            foreach(KeyValuePair<string, SpreadsheetData> spreadsheet in _data)
            {
                string data = GetData(spreadsheet.Key, key);
                if (!string.IsNullOrEmpty(data)) return data;
            }

            return null;
        }

        #endregion
    }

    internal class SpreadsheetData
    {
        public string SpreadsheetName;
        public Dictionary<string, SpreadsheetRow> Data;
    }

    internal class SpreadsheetRow
    {
        public string Key;
        public Dictionary<string, string> LangTexts;
    }
}