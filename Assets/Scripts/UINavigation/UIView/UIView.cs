using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace uinavigation.uiview
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] float _animDuration;

        protected List<UITransitionBase> _uiTransitions;
        /// <summary>
        /// UIView의 화면 전환 애니메이션을 담은 UITransitionBase 리스트
        /// </summary>
        public List<UITransitionBase> UITransitions
        {
            get
            {
                if (_uiTransitions == null)
                {
                    _uiTransitions = new List<UITransitionBase>();
                    _uiTransitions = GetComponents<UITransitionBase>().ToList();
                }

                // 특정 UITransition가 없다면, UIFadeInOut을 추가한다.
                if (_uiTransitions.Count == 0)
                    _uiTransitions.Add(gameObject.AddComponent<UIFadeInOut>());

                return _uiTransitions;
            }
        }
        /// <summary>
        /// 해당 UIView의 현재 화면 전환 상태. 가장 첫 번째 UITransitionBase의 상태를 따른다.
        /// </summary>
        public VisibleState VisibleState => UITransitions.FirstOrDefault().VisibleState;
        /// <summary>
        /// 해당 UIView의 화면 전환 상태가 바뀔 때 호출되는 이벤트. 가장 첫 번째 UITransitionBase의 이벤트를 따른다.
        /// </summary>
        public UnityEvent<VisibleState> OnChangedVisibleState => UITransitions.FirstOrDefault().OnChangedVisibleState;
        protected CanvasGroup CanvasGroup => UITransitions.FirstOrDefault().CanvasGroup;
        private RectTransform _rectTransform;

        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = Vector2.zero;

            // 초기 모든 UIView는 화면에서 숨겨진 상태로 시작한다.
            UITransitions.ForEach(transition => transition.HideImmediately());
        }

        protected virtual void Start()
        {
            UITransitions.FirstOrDefault().OnChangedVisibleState.AddListener(OnChangeVisibleState);
        }

        private void OnChangeVisibleState(VisibleState state)
        {
            switch (state)
            {
                case VisibleState.Appeared:
                    OnShow();
                    break;
                case VisibleState.Appearing:
                    OnShowing();
                    break;
                case VisibleState.Disappeared:
                    OnHide();
                    break;
                case VisibleState.Disappearing:
                    OnHiding();
                    break;
            }
        }

        public virtual async UniTask Show()
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            if (UITransitions != null)
            {
                List<UniTask> tasks = new List<UniTask>();
                UITransitions.ForEach(transition => tasks.Add(transition.Show(_animDuration)));
                await UniTask.WhenAll(tasks);
            }
        }

        public virtual async UniTask Hide()
        {
            if (UITransitions != null)
            {
                List<UniTask> tasks = new List<UniTask>();
                UITransitions.ForEach(transition => tasks.Add(transition.Hide(_animDuration)));
                await UniTask.WhenAll(tasks);
            }
        }

        protected virtual void OnShow()
        {
            this.CanvasGroup.interactable = true;
        }

        protected virtual void OnShowing()
        {
            this.CanvasGroup.interactable = true;
        }

        protected virtual void OnHide()
        {
            this.CanvasGroup.interactable = false;
        }

        protected virtual void OnHiding()
        {
            this.CanvasGroup.interactable = false;
        }
    }

}
