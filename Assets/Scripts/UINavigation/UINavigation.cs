using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using uinavigation.uiview;

namespace uinavigation
{
    public partial class UINavigation
    {
        private static UINavigation _instance;
        private Queue<(UIView hide, UIView show)> _uiViewTransQueue;
        private Stack<UIView> _views;
        private UIView _currentView;

        public static UIView CurrentView => _instance?._currentView;

        private UINavigation()
        {
            UIContextManager.Initialize();

            _views = new Stack<UIView>();
            _uiViewTransQueue = new Queue<(UIView hide, UIView show)>();
            Observable.EveryUpdate().Where(x => _uiViewTransQueue.Count > 0)
                .Subscribe(y =>
                {
                    if (_currentView == null || _currentView.VisibleState == VisibleState.Appeared)
                    {
                        var dequeuedItem = _uiViewTransQueue.Dequeue();
                        TransUIView(dequeuedItem.hide, dequeuedItem.show);
                    }
                });
        }

        private async void TransUIView(UIView hide, UIView show)
        {
            if (hide != null) await hide.Hide();
            _currentView = show;
            if (show != null) await show.Show();

        }

        public static UIView PushUIView(string viewName)
        {
            if (_instance == null)
                _instance = new UINavigation();

            UIView show = UIViewContainer.GetUIView(viewName);

            if (show == null)
            {
                Debug.LogWarning($"{viewName}의 UIView를 찾을 수 없습니다.");
                return null;
            }
            _instance._views.TryPeek(out UIView hide);
            _instance._views.Push(show);
            _instance._uiViewTransQueue.Enqueue((hide, show));
            return show;
        }

        public static async Task<UIView> PushUIViewAsync(string viewName)
        {
            if (_instance == null)
                _instance = new UINavigation();
            UIView show = UIViewContainer.GetUIView(viewName);
            if (show == null)
            {
                Debug.LogWarning($"{viewName}의 UIView를 찾을 수 없습니다.");
                return null;
            }

            _instance._views.Push(show);
            await show.Show();
            _instance._currentView = show;
            return show;
        }

        public static UIView PopUIView()
        {
            if (_instance == null)
                _instance = new UINavigation();
            if (_instance._views == null || _instance._views.Count == 0)
            {
                Debug.LogWarning("UIView가 더이상 존재하지 않습니다.");
                return null;
            }
            _instance._views.TryPop(out UIView hide);
            _instance._views.TryPeek(out UIView show);
            _instance._uiViewTransQueue.Enqueue((hide, show));
            return hide;
        }

        public static async Task<UIView> PopUIViewAsync()
        {
            if (_instance == null)
                _instance = new UINavigation();
            if (_instance._views == null || _instance._views.Count == 0)
            {
                Debug.LogWarning("UIView가 더이상 존재하지 않습니다.");
                return null;
            }
            _instance._views.TryPop(out UIView hide);
            await hide.Hide();
            _instance._currentView = null;
            return hide;
        }

        public static UIView PopToUIView(string viewName)
        {
            if (_instance == null)
                _instance = new UINavigation();
            if (_instance._views == null || _instance._views.Count == 0)
            {
                Debug.LogWarning("UIView가 더이상 존재하지 않습니다.");
                return null;
            }

            if (!_instance._views.Any(view => view.name == viewName))
            {
                Debug.LogWarning($"{viewName}의 UIView를 찾을 수 없습니다.");
                return null;
            }

            if (_instance._views.Last().name == viewName)
            {
                Debug.LogWarning($"{viewName}의 UIView는 가장 최근에 Push한 UIView입니다.");
                return null;
            }

            _instance._views.TryPop(out UIView hide);
            while (_instance._views.Count > 0)
            {
                _instance._views.TryPeek(out UIView show);
                if (show.name == viewName)
                {
                    _instance._uiViewTransQueue.Enqueue((hide, show));
                    break;
                }
                _instance._views.Pop();
            }
            return hide;
        }

        public static UIView PopToRoot()
        {
            if (_instance == null)
                _instance = new UINavigation();
            if (_instance._views == null || _instance._views.Count == 0)
            {
                Debug.LogWarning("UIView가 더이상 존재하지 않습니다.");
                return null;
            }

            if (_instance._views.Count < 2)
            {
                Debug.LogWarning("UIView 개수가 2개 이상이어야 합니다.");
                return null;
            }

            _instance._views.TryPop(out UIView hide);
            while (_instance._views.Count > 1)
            {
                _instance._views.Pop();
            }
            _instance._views.TryPeek(out UIView show);
            _instance._uiViewTransQueue.Enqueue((hide, show));
            return hide;
        }

        public static void Dispose()
        {
            _instance = null;
        }
    }
}