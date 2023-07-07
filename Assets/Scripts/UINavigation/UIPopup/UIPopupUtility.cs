using UnityEngine.Events;
using uinavigation.uiview;

namespace uinavigation.popup
{
    public static class UIPopupUtility
    {
        public static T SetText<T>(this T uiPopup, params string[] texts) where T : UIPopup
        {
            if (texts.Length == 0) return uiPopup;
            if (uiPopup.TextFields.Count == 0) return uiPopup;

            for (int idx = 0; idx < uiPopup.TextFields.Count; idx++)
            {
                if (idx > texts.Length - 1) break;
                uiPopup.TextFields[idx].text = texts[idx];
                uiPopup.TextFields[idx].ForceMeshUpdate();
            }
            return uiPopup;
        }

        public static T SetButtonEvent<T>(this T uiPopup, params UnityAction[] actions) where T : UIPopup
        {
            if (actions.Length == 0) return uiPopup;
            if (uiPopup.Buttons.Count == 0) return uiPopup;

            for (int idx = 0; idx < uiPopup.Buttons.Count; idx++)
            {
                if (idx > actions.Length - 1) break;
                uiPopup.Buttons[idx].onClick.AddListener(actions[idx]);
            }

            return uiPopup;
        }

        public static T SetDependencyOnView<T>(this T uiPop, UIView view) where T : UIPopup
        {
            uiPop.View = view;
            return uiPop;
        }
    }
}
