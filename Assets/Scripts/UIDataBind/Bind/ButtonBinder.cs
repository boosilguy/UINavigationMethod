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

        private UnityAction prevAction;

        public void Bind(DataContext context)
        {
            if (context.ContainsKey(_key))
            {
                if (prevAction != null)
                    _button.onClick.RemoveListener(prevAction);

                prevAction = ((UnityAction)context[_key]);

                _button.onClick.AddListener(() =>
                {
                    prevAction.Invoke();
                });
            }
                
        }
    }
}