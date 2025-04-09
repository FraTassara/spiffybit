using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SpiffyBit.Localization
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Localization/Localized Text")]
    public class LocalizationText : MonoBehaviour
    {
        [SerializeField] private string _key;
        [SerializeField] private string _alternativeText;

        private bool _subscribed = false;

        private TextMeshProUGUI _tmpField;
        private Text _textField;

        private void Start()
        {
            _tmpField = GetComponent<TextMeshProUGUI>();
            _textField = GetComponent<Text>();

            Subscribe();
            Refresh();
        }

        private void Refresh()
        {
            string result = Localization.Get(_key, _alternativeText);
            if (_tmpField != null) _tmpField.text = result;
            if (_textField != null) _textField.text = result;
        }

        private void Subscribe()
        {
            Localization.OnLanguageChanged += Refresh;
            _subscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_subscribed) return;
            Localization.OnLanguageChanged -= Refresh;
        }

        private void OnDestroy() => Unsubscribe();
    }
}