using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using System;

namespace uidatabind
{
    [RequireComponent(typeof (TextMeshProUGUI))]
    public class TextMeshProBinder : MonoBehaviour, IBindable
    {
        private TextMeshProUGUI _textComponent;
        private string _text;

        public string Key
        {
            get => null;
        }

        public void Bind(DataContext context)
        {
            if (_text == null)
            {
                _textComponent = GetComponent<TextMeshProUGUI>();
                _text = _textComponent.text;
            }

            _textComponent.text = Regex.Replace(_text, UIDataBind.BIND_REGEX, match =>
            {
                var splited = match.Value.Substring(2, match.Value.Length - 4).Split(':');
                var key = splited.FirstOrDefault();
                var format = splited.Skip(1).FirstOrDefault();
                var value = context[key];

                if (splited.Count() == 2 && value is IFormattable)
                    return ((IFormattable)value).ToString(format, System.Globalization.CultureInfo.CurrentCulture);

                return value.ToString();
            });
        }

        private bool IsRegistered(MatchCollection matchCollection, DataContext context)
        {
            bool isContain = false;
            matchCollection.ToList().ForEach(match =>
            {
                // 현재 정규식 결과로부터, 키 값을 가져온다.
                // 이를테면, {&MySpecialKey:N0&}의 결과로부터 MySpecialKey를 가져온다.
                var key = match.Value.Substring(2, match.Value.Length - 4).Split(':').FirstOrDefault();
                if (context.ContainsKey(key))
                {
                    isContain = true;
                    return;
                }
            });

            return isContain;
        }
    }
}

