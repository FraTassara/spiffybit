using System.IO;
using UnityEditor;
using UnityEngine;

namespace SpiffyBit.Localization.Utility
{
    internal static class ExtractorTool
    {
        public static void ExtractCharacters(TextAsset targetFile, SystemLanguage systemLanguage)
        {
            LanguageCode languageCode = systemLanguage.ToLanguageCode();
            string path = AssetDatabase.GetAssetPath(targetFile);
            string data = DataUtility.GetAllCharacters(languageCode);

            if (!CheckContent(data)) return;

            CreateDataAsset(path, data);
            Debug.Log($"Extracted {systemLanguage} language to {path}");
        }

        private static bool CheckContent(string content)
        {
            bool success = !string.IsNullOrEmpty(content);
            if (!success) Debug.Log("Selected language not exist in your files.");
            return success;
        }

        private static void CreateDataAsset(string path, string content)
        {
            File.WriteAllText(path, content);
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(path);
        }
    }
}