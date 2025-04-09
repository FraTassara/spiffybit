using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SpiffyBit.Localization
{
    [CustomPropertyDrawer(typeof(LanguageCode))]
    internal class LanguageCode_PropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            //Create Container
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetsPaths.LanguageCode);
            var container = visualTreeAsset.CloneTree().Q("Container");

            //Bind Language Enum
            var languageEnumField = new EnumField("Language");
            languageEnumField.BindProperty(property.FindPropertyRelative("Language"));
            container.Add(languageEnumField);

            //Bind String Code
            var textField = new TextField("Code");
            textField.BindProperty(property.FindPropertyRelative("Code"));
            container.Add(textField);

            return container;
        }
    }
}