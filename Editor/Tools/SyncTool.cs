using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace SpiffyBit.Localization.Utility
{
    internal static class SyncAssets
    {
        private static string DataFolderName => "Localization Data";

        public static async void SyncLocalizationData(LocalizationSettings localizationSettings)
        {
            bool hasSpreadsheetId = !string.IsNullOrEmpty(localizationSettings.SpreadsheetID);
            if (!hasSpreadsheetId) Debug.LogWarning("Spreadsheet ID field is empty. Add google spreadsheet ID.");

            bool hasSpreadsheetNames = localizationSettings.SpreadsheetNames.Count > 0;
            if(!hasSpreadsheetNames) Debug.LogWarning("Spreadsheet name list is empty. Add spreadsheet names to sync data.");

            if (!hasSpreadsheetId || !hasSpreadsheetNames) return;

            List<(string, string)> data = new List<(string, string)>();
            foreach (string spreadsheetName in localizationSettings.SpreadsheetNames)
            {
                if (string.IsNullOrEmpty(spreadsheetName)) continue;

                var result = await DownloadData(localizationSettings.SpreadsheetID, spreadsheetName);
                if (!string.IsNullOrEmpty(result)) data.Add((spreadsheetName, result));
            }

            bool hasDownloadedData = data.Count > 0;
            if (hasDownloadedData) SaveData(localizationSettings, data);
            else Debug.LogWarning("No data. Make sure your sheet names are correct.");
        }

        private static async Task<string> DownloadData(string sheetId, string sheetName)
        {
            string data = null;
            string url = $"https://opensheet.elk.sh/{sheetId}/{sheetName}";
            using (var webRequest = UnityWebRequest.Get(url))
            {
                var operation = webRequest.SendWebRequest();
                while (!operation.isDone)
                {
                    EditorUtility.DisplayProgressBar("Sync Data Progress", $"Downloading {sheetName}...", operation.progress);
                    await Task.Yield();
                }

                if (webRequest.result != UnityWebRequest.Result.Success) Debug.LogWarning($"Sync with {sheetName} failed!");
                else data = webRequest.downloadHandler.text;

                EditorUtility.ClearProgressBar();
            }

            return data;
        }

        private static void SaveData(LocalizationSettings localizationSettings, List<(string, string)> data)
        {
            foreach(var spreadsheetData in data)
            {
                if (TryToFindInAssetList(localizationSettings, spreadsheetData.Item1, spreadsheetData.Item2)) continue;
                if (TryToFindInResources(localizationSettings, spreadsheetData.Item1, spreadsheetData.Item2)) continue;

                CreateLocalizationAsset(localizationSettings, spreadsheetData.Item1, spreadsheetData.Item2);
            }

            localizationSettings.Spreadsheets = localizationSettings.Spreadsheets.Where(x => x != null).ToList();
        }

        private static bool TryToFindInAssetList(LocalizationSettings localizationSettings, string spreadsheetName, string spreadsheetContent)
        {
            foreach(TextAsset textAsset in localizationSettings.Spreadsheets)
            {
                if (textAsset == null || textAsset.name != spreadsheetName) continue;

                var path = AssetDatabase.GetAssetPath(textAsset);
                File.WriteAllText(path, spreadsheetContent);

                EditorUtility.SetDirty(textAsset);
                AssetDatabase.SaveAssets();
                AssetDatabase.ImportAsset(path);

                return true;
            }

            return false;
        }

        private static bool TryToFindInResources(LocalizationSettings localizationSettings, string spreadsheetName, string spreadsheetContent)
        {
            var textAssets =  Resources.LoadAll<TextAsset>($"{DataFolderName}/");
            foreach (TextAsset textAsset in textAssets)
            {
                if (textAsset == null || textAsset.name != spreadsheetName) continue;

                var path = AssetDatabase.GetAssetPath(textAsset);
                File.WriteAllText(path, spreadsheetContent);

                EditorUtility.SetDirty(textAsset);
                AssetDatabase.SaveAssets();
                AssetDatabase.ImportAsset(path);

                AddNewAssetToList(localizationSettings, path);

                return true;
            }

            return false;
        }

        private static void CreateLocalizationAsset(LocalizationSettings localizationSettings, string spreadsheetName, string spreadsheetContent)
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources")) AssetDatabase.CreateFolder("Assets", "Resources");
            if (!AssetDatabase.IsValidFolder($"Assets/Resources/{DataFolderName}")) AssetDatabase.CreateFolder("Assets/Resources", DataFolderName);

            string path = $"Assets/Resources/{DataFolderName}/{spreadsheetName}.json";
            File.WriteAllText(path, spreadsheetContent);

            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(path);

            AddNewAssetToList(localizationSettings, path);
        }

        private static void AddNewAssetToList(LocalizationSettings localizationSettings, string path)
        {
            localizationSettings.Spreadsheets.Add(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
            EditorUtility.SetDirty(localizationSettings);
            AssetDatabase.SaveAssets();
        }
    }
}