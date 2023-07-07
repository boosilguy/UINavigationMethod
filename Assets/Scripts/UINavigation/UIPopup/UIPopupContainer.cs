using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace uinavigation.popup
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public class UIPopupContainer : MonoBehaviour
    {
        [SerializeField] private List<UIPopup> _uiPopupList;

        private static UIPopupContainer _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Debug.LogError($"{this.GetType()}은 Scene에 1개만 할당할 수 있습니다. {this.gameObject}의 Component는 무시 처리합니다.");
        }

        public static UIPopup GetUIPopup(string popupName)
        {
            var popup = _instance._uiPopupList.SingleOrDefault(found => found.name == popupName);
            if (popup == null)
            {
                Debug.LogWarning($"{popupName}의 Popup UI를 찾을 수 없습니다.");
                return null;
            }

            popup = Instantiate(popup, _instance.transform, false);
            popup.HideImmediately();
            return popup;
        }
    }
}
