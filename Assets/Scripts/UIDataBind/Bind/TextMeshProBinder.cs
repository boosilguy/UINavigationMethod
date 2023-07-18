using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

namespace uidatabind
{
    public class TextMeshProBinder : MonoBehaviour, IBindable
    {
        private string _text;
        private TextMeshProUGUI _textComponent;
        public string key
        {
            get { return null; }
        }
        public void Bind(DataContext context)
        {
            if (_text == null)
            {
                _textComponent = GetComponent<TextMeshProUGUI>();
                _text = _textComponent.text;
            }

            var matches = Regex.Matches(_text, @"\{\{[^}]*}}");
            var any = false;
            for (var i = 0; i < matches.Count; i++)
            {
                var m = matches[i];
                var key = m.Value.Substring(2, m.Value.Length - 4)
                           .Split(':')[0];
                if (context.ContainsKey(key))
                {
                    any = true;
                    break;
                }
            }
            if (!any)
            {
                return;
            }
            _textComponent.text = Regex.Replace(_text, @"\{\{[^}]*}}", m =>
            {
                var target = m.Value.Substring(2, m.Value.Length - 4)
                              .Split(':');
                var key = target[0];
                var val = context[key];
                if (target.Length == 2 && context[key] is System.IFormattable)
                {
                    var format = target[1];
                    return ((System.IFormattable)val).ToString(format, System.Globalization.CultureInfo.CurrentCulture);
                }
                return val.ToString();
            });
        }
    }
    }
}

