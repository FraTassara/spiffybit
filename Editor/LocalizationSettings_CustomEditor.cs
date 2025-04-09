using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using SpiffyBit.Localization.Utility;

namespace SpiffyBit.Localization
{
    [CustomEditor(typeof(LocalizationSettings))]
    internal class LocalizationSettings_CustomEditor : Editor
    {
        private VisualElement _rootVisualElement;
        private LocalizationSettings _localizationSettings;

        private void OnEnable() => _localizationSettings = target as LocalizationSettings;

        public override VisualElement CreateInspectorGUI()
        {
            InitRoot();
            InitToolbar();
            InitSyncData();
            InitExtracotr();
            InitHelpboxes();

            return _rootVisualElement;
        }

        private void InitRoot()
        {
            _rootVisualElement = new VisualElement();
            VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetsPaths.LocalizationWindow);
            visualTreeAsset.CloneTree(_rootVisualElement);
        }

        private void InitToolbar()
        {
            _rootVisualElement.Q<Button>("DataButton").clicked += () => DisplayTab(LocalizationTab.Localization);
            _rootVisualElement.Q<Button>("CodesButton").clicked += () => DisplayTab(LocalizationTab.Codes);
            _rootVisualElement.Q<Button>("SyncButton").clicked += () => DisplayTab(LocalizationTab.Sync);
            _rootVisualElement.Q<Button>("ExtractorButton").clicked += () => DisplayTab(LocalizationTab.Extractor);

            DisplayTab(LocalizationTab.Sync);
        }

        private void InitHelpboxes()
        {
            _rootVisualElement.Q<VisualElement>("Assets").Add(new HelpBox(AssetsHelpInfo, HelpBoxMessageType.Info));
            _rootVisualElement.Q<VisualElement>("Codes").Add(new HelpBox(CodesHelpInfo, HelpBoxMessageType.Info));
            _rootVisualElement.Q<VisualElement>("Sync").Add(new HelpBox(SyncHelpInfo, HelpBoxMessageType.Info));
            _rootVisualElement.Q<VisualElement>("Extractor").Add(new HelpBox(ExtractorHelpInfo, HelpBoxMessageType.Info));
        }

        private void InitExtracotr()
        {
            _rootVisualElement.Q<Button>("ExtractButton").clicked += () =>
            {
                var systemLanguage = (SystemLanguage)_rootVisualElement.Q<EnumField>("ExtractorEnum").value;
                var textAsset = _rootVisualElement.Q<ObjectField>("ExtractorObject").value as TextAsset;
                if (textAsset != null) ExtractorTool.ExtractCharacters(textAsset, systemLanguage);
            };
        }

        private void InitSyncData()
        {
            _rootVisualElement.Q<Button>("SyncDataButton").clicked += () =>
            {
                SyncAssets.SyncLocalizationData(_localizationSettings);
            };
        }

        private void DisplayTab(LocalizationTab tab)
        {
            _rootVisualElement.Q<VisualElement>("Assets").style.display = (DisplayStyle)(tab == LocalizationTab.Localization ? 0 : 1);
            _rootVisualElement.Q<VisualElement>("Codes").style.display = (DisplayStyle)(tab == LocalizationTab.Codes ? 0 : 1);
            _rootVisualElement.Q<VisualElement>("Sync").style.display = (DisplayStyle)(tab == LocalizationTab.Sync ? 0 : 1);
            _rootVisualElement.Q<VisualElement>("Extractor").style.display = (DisplayStyle)(tab == LocalizationTab.Extractor ? 0 : 1);
        }

        private string AssetsHelpInfo = "All your translation files are located here.";
        private string CodesHelpInfo = "Here are all your supported language codes. Your defined codes must match the codes on the sheet.";
        private string SyncHelpInfo => "You can sync your google spreadsheets here. Remember that your sheets must have the same name to be overwritten.";
        private string ExtractorHelpInfo = "Useful tool when using Text Mesh Pro with custom characters. It will extract all characters from your sheets.";
    }

    internal static class AssetsPaths
    {
        internal static string LocalizationWindow => "Packages/com.spiffybit.localization/Editor/UI/LocalizationWindow.uxml";
        internal static string LanguageCode => "Packages/com.spiffybit.localization/Editor/UI/LanguageCode.uxml";
    }

    internal enum LocalizationTab { Localization, Codes, Sync, Extractor }
}