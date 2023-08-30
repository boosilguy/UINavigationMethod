using DG.Tweening;
using UnityEngine;
using uinavigation.uiview;

namespace uinavigation
{
    /// <summary>
    /// UI 라이프 사이클을 관리하는 클래스
    /// </summary>
    public class UIContextManager : MonoBehaviour
    {
        private static UIContextManager _instance;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns>UIContextManager</returns>
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

        /// <summary>
        /// Destroy시, DOTween, UINavigation, UIViewContainer를 Dispose합니다.
        /// </summary>
        private void OnDestroy()
        {
            DOTween.Clear();
            UINavigation.Dispose();
            UIViewContainer.Dispose();
        }
    }
}
