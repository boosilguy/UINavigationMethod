using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace uinavigation
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UITransitionBase : MonoBehaviour
    {
        public UnityEvent<VisibleState> OnChangedVisibleState = new UnityEvent<VisibleState>();

        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup
        {
            get
            {
                _canvasGroup ??= GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        private VisibleState _visibleState = VisibleState.Appeared;
        public VisibleState VisibleState
        {
            get => _visibleState;
            set
            {
                if (_visibleState == value)
                    return;
                _visibleState = value;

                if (CanvasGroup != null)
                {
                    CanvasGroup.blocksRaycasts = (_visibleState == VisibleState.Appeared);
                }

                switch (_visibleState)
                {
                    case VisibleState.Appeared:
                        OnShow(); break;
                    case VisibleState.Appearing:
                        OnShowing(); break;
                    case VisibleState.Disappearing:
                        OnHiding(); break;
                    case VisibleState.Disappeared:
                        OnHide(); break;
                }
                OnChangedVisibleState?.Invoke(_visibleState);
            }
        }

        private bool initialized = false;

        public void ShowImmediately()
        {
            Show().Forget();
        }

        public async UniTask Show(float duration = 0)
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }

            switch (_visibleState)
            {
                case VisibleState.Appeared:
                case VisibleState.Appearing:
                    return;
                case VisibleState.Disappearing:
                    KillAnim();
                    break;
            }

            VisibleState = VisibleState.Appearing;

            if (duration == 0)
            {
                ShowWithoutAnim();
            }
            else
            {
                await ShowAnim(duration);
            }

            if (VisibleState == VisibleState.Appearing)
                VisibleState = VisibleState.Appeared;
        }

        public void HideImmediately()
        {
            Hide().Forget();
        }

        public async UniTask Hide(float duration = 0)
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }

            switch (_visibleState)
            {
                case VisibleState.Disappeared:
                case VisibleState.Disappearing:
                    return;
                case VisibleState.Appearing:
                    KillAnim();
                    break;
            }

            VisibleState = VisibleState.Disappearing;

            if (duration == 0)
            {
                HideWithoutAnim();
            }
            else
            {
                await HideAnim(duration);
            }

            if (VisibleState == VisibleState.Disappearing)
                VisibleState = VisibleState.Disappeared;
        }

        protected virtual void Initialize() { }

        protected abstract UniTask ShowAnim(float duration);
        protected abstract UniTask HideAnim(float duration);
        protected abstract void ShowWithoutAnim();
        protected abstract void HideWithoutAnim();
        protected abstract void KillAnim();

        protected virtual void OnShow() { }

        protected virtual void OnShowing() { }

        protected virtual void OnHide() { }

        protected virtual void OnHiding() { }
    }

}
