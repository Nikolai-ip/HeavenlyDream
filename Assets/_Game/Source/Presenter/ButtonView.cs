using System;
using Plugins.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Source.Presenter
{
    public class ButtonView: MonoBehaviour, IViewInteractable
    {
        public event Action Callback;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(() => Callback?.Invoke());
        }
        
    }
}