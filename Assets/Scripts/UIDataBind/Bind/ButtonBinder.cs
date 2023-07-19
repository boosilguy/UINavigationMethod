using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace uidatabind
{
    public class ButtonBinder : MonoBehaviour, IBindable
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _key;

        public string Key => _key;

        public void Bind(DataContext context)
        {
            if (context.ContainsKey(_key))
                _button.onClick.AddListener((UnityAction)context[_key]);
        }
    }
}