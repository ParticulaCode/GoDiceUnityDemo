using FrostLib.Signals.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    internal class DeviceView : MonoBehaviour
    {
        public readonly Signal OnClick = new Signal();

        [SerializeField] private Button _btn;
        [SerializeField] private TMP_Text _titleLabel;

        public void Initialize(string address, string deviceName)
        {
            _titleLabel.text = $"{deviceName} ({address})";
            _btn.onClick.AddListener(OnClick.Dispatch);
        }

        private void OnDestroy()
        {
            _btn.onClick.RemoveAllListeners();
            OnClick.ClearListeners();
        }
    }
}