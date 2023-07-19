using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace uidatabind
{
    public class ImageBinder : MonoBehaviour, IBindable
    {
        [SerializeField] private Image _image;
        [SerializeField] private string _key;

        public string Key => _key;

        public void Bind(DataContext context)
        {
            if (context.ContainsKey(_key))
                _image.sprite = (Sprite)context[_key];
        }
    }
}

