using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace uinavigation.uiview
{
    public class UIViewContainer
    {
        private static UIViewContainer _instance = null;
        private Dictionary<string, UIView> _uiViewDic = null;

        private UIViewContainer()
        {
            if (_uiViewDic != null)
                return;

            _uiViewDic = GameObject.FindObjectsOfType<UIView>(true).ToDictionary(view => view.name, view => view);
        }

        public static UIView GetUIView(string name)
        {
            if (_instance == null)
                _instance = new UIViewContainer();

            if (_instance._uiViewDic.ContainsKey(name))
                return _instance._uiViewDic[name];

            Debug.LogError($"명명된 UIView ({name})가 존재하지 않습니다.");
            return null;
        }

        public static void Dispose()
        {
            if (_instance != null && _instance._uiViewDic != null)
            {
                _instance._uiViewDic.Clear();
                _instance._uiViewDic = null;
            }
            _instance = null;
        }
    }
}
