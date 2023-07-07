using DG.Tweening;
using UnityEngine;
using uinavigation.uiview;

namespace uinavigation
{
    public class UIContextManager : MonoBehaviour
    {
        private static UIContextManager _instance;

        public static UIContextManager Initialize()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIContextManager>();
                if (_instance == null)
                    _instance = new GameObject(UINavigation.UIContextManager_GameObject).AddComponent<UIContextManager>();
            }
            return _instance;
        }

        private void OnDestroy()
        {
            DOTween.Clear();
            UINavigation.Dispose();
            UIViewContainer.Dispose();
        }
    }
}
