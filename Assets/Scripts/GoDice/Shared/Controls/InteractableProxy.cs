using System;
using FrostLib.Signals.impl;
using UnityEngine;
using UnityEngine.UI;

namespace GoDice.Shared.Controls
{
    public class InteractableProxy : IDisposable
    {
        public readonly Signal OnClick = new Signal();

        private readonly Button _unityBtn;

        public InteractableProxy(GameObject go)
        {
            _unityBtn = go.GetComponent<Button>();
            if (_unityBtn != null)
            {
                _unityBtn.onClick.AddListener(OnClick.Dispatch);
                return;
            }

            Debug.LogWarning($"No Button component found to subscribe", go);
        }

        public void SetInteractable(bool isOn)
        {
            if (_unityBtn)
                _unityBtn.interactable = isOn;
        }

        public void Dispose()
        {
            if (_unityBtn)
                _unityBtn.onClick.RemoveListener(OnClick.Dispatch);

            OnClick.ClearListeners();
        }
    }
}