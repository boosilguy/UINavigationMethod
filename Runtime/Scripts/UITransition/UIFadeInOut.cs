using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace uinavigation
{
    /// <summary>
    /// UI Fade In & Out 애니메이션을 적용하는 UITransition
    /// </summary>
    public class UIFadeInOut : UITransitionBase
    {
        private Tween tween;

        protected override UniTask HideAnim(float duration)
        {
            tween = CanvasGroup.DOFade(0, duration).SetAutoKill();
            return tween.AsyncWaitForCompletion().AsUniTask();
        }

        protected override void HideWithoutAnim()
        {
            CanvasGroup.alpha = 0;
        }

        protected override void KillAnim()
        {
            if (tween.active) tween.Kill(true);
        }

        protected override UniTask ShowAnim(float duration)
        {
            tween = CanvasGroup.DOFade(1, duration).SetAutoKill();
            return tween.AsyncWaitForCompletion().AsUniTask();
        }

        protected override void ShowWithoutAnim()
        {
            CanvasGroup.alpha = 1;
        }
    }
}
