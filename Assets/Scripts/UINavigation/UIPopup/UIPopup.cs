using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using uinavigation.uiview;

namespace uinavigation.popup
{
    public class UIPopup : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _contentCanvasGroup;
        [SerializeField] protected CanvasGroup _backgroundCanvasGroup;
        [SerializeField] protected float _animDuration = 1f;
        [SerializeField] protected bool _setHideFuncToLastButton;

        [Header("UI Components")]

        [Tooltip("동적으로 Text 내용을 할당할 TMP TextField List입니다.")]
        [SerializeField] private List<TextMeshProUGUI> _textFields;
        public List<TextMeshProUGUI> TextFields => _textFields;

        [Tooltip("동적으로 Listener를 할당할 Button List입니다.")]
        [SerializeField] private List<Button> _buttons;
        public List<Button> Buttons => _buttons;

        private static UIPopup _instance;
        public static UIPopup Instance => _instance;
        public VisibleState VisibleState { get; private set; } = VisibleState.Disappeared;
        private UIView _view;
        public UIView View
        {
            get => _view;
            set
            {
                _view?.OnChangedVisibleState.RemoveListener(OnViewVisibleStateChanged);
                _view = value;
                _view?.OnChangedVisibleState.AddListener(OnViewVisibleStateChanged);
            }
        }

        public static UIPopup GetUIPopup(string name)
        {
            var uiPopup = UIPopupContainer.GetUIPopup(name);
            if (uiPopup == null)
            {
                Debug.LogWarning($"{name}의 UIPopup이 Container 목록에 존재하지 않습니다.");
                return null;
            }

            var contentUITransition = uiPopup._contentCanvasGroup?.GetComponent<UITransitionBase>();
            if (contentUITransition == null)
                contentUITransition = uiPopup._contentCanvasGroup.gameObject.AddComponent<UIFadeInOut>();
            contentUITransition.HideImmediately();

            if (uiPopup._backgroundCanvasGroup != null)
            {
                var backgroundUITransition = uiPopup._backgroundCanvasGroup.GetComponent<UITransitionBase>();
                if (backgroundUITransition == null)
                    backgroundUITransition = uiPopup._backgroundCanvasGroup.gameObject.AddComponent<UIFadeInOut>();
                backgroundUITransition.HideImmediately();
            }

            uiPopup.name = name;
            uiPopup.SetHideFuncToLastButton();
            return uiPopup;
        }

        public void ShowImmediately()
        {
            Show(0).Forget();
        }

        public async virtual UniTask Show()
        {
            _instance = this;
            await PopUp(_animDuration);
        }

        public async virtual UniTask Show(float duration)
        {
            _instance = this;
            await PopUp(duration);
        }

        public void HideImmediately()
        {
            Hide(0).Forget();
        }

        public async virtual UniTask Hide()
        {
            if (_instance != null)
            {
                await _instance.Dismiss(_animDuration);
                Destroy();
            }
        }

        public async virtual UniTask Hide(float duration)
        {
            if (_instance != null)
            {
                await _instance.Dismiss(duration);
                Destroy();
            }
        }

        public static void Destroy()
        {
            if (_instance != null && _instance.VisibleState == VisibleState.Disappeared)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }

        private void SetHideFuncToLastButton()
        {
            if (_setHideFuncToLastButton)
            {
                var buttons = _contentCanvasGroup.GetComponentsInChildren<Button>();
                if (buttons.Length == 0)
                {
                    Debug.LogWarning($"{this.name}에 UIPopup {this.GetType()}의 Set Hide Func To Last Button 속성이 활성화되지 않았습니다 (Content Canvas의 자식 Button count가 0).");
                    return;
                }
                buttons.Last().onClick.AddListener(() => Hide().Forget());
            }
        }

        private void OnViewVisibleStateChanged(VisibleState visibleState)
        {
            if (visibleState == VisibleState.Disappearing)
                Dismiss(_animDuration).Forget();

        }

        protected virtual async UniTask PopUp(float duration)
        {
            List<UniTask> appearAnimJobs = new List<UniTask>();
            switch (this.VisibleState)
            {
                case VisibleState.Appeared:
                case VisibleState.Appearing:
                    return;
            }

            this.VisibleState = VisibleState.Appearing;

            // Show UI Animation 설정
            if (_contentCanvasGroup != null)
                AddShowAnimationJobs(appearAnimJobs, duration, _contentCanvasGroup);

            if (_backgroundCanvasGroup != null)
                AddShowAnimationJobs(appearAnimJobs, duration, _backgroundCanvasGroup);

            if (_contentCanvasGroup == null)
            {
                Debug.LogWarning($"{this.name}의 {this.GetType()}에 Content Canvas Group이 null입니다.");
            }

            await UniTask.WhenAll(appearAnimJobs);

            this.VisibleState = VisibleState.Appeared;
        }

        protected virtual async UniTask Dismiss(float duration)
        {
            List<UniTask> appearAnimJobs = new List<UniTask>();
            switch (this.VisibleState)
            {
                case VisibleState.Disappeared:
                case VisibleState.Disappearing:
                    return;
            }

            this.VisibleState = VisibleState.Disappearing;

            // Show UI Animation 설정
            if (_contentCanvasGroup != null)
                AddHideAnimationJobs(appearAnimJobs, duration, _contentCanvasGroup);

            if (_backgroundCanvasGroup != null)
                AddHideAnimationJobs(appearAnimJobs, duration, _backgroundCanvasGroup);

            if (_contentCanvasGroup == null)
            {
                Debug.LogWarning($"{this.name}의 {this.GetType()}에 Content Canvas Group이 null입니다.");
            }

            await UniTask.WhenAll(appearAnimJobs);

            this.VisibleState = VisibleState.Disappeared;
        }

        protected virtual void AddShowAnimationJobs(List<UniTask> jobs, float duration, CanvasGroup target)
        {
            if (target != null)
            {
                var uiTransition = target.GetComponent<UITransitionBase>();
                jobs.Add(uiTransition.Show(duration));
            }
        }

        protected virtual void AddHideAnimationJobs(List<UniTask> jobs, float duration, CanvasGroup target)
        {
            if (target != null)
            {
                var uiTransition = target.GetComponent<UITransitionBase>();
                jobs.Add(uiTransition.Hide(duration));
            }
        }
    }
}
