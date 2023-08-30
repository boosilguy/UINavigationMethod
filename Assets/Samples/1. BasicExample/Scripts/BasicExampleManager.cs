using Cysharp.Threading.Tasks;
using UnityEngine;
using uinavigation;
using uinavigation.popup;

namespace example
{
    public class BasicExampleManager : MonoBehaviour
    {
        private static BasicExampleManager _instance;
        public static BasicExampleManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<BasicExampleManager>();
                return _instance;
            }
        }

        private void Start()
        {
            UINavigation.PushUIView("MainView");
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Backspace))
                UINavigation.PopUIView();
        }

        public void OnClickNextButton(GameObject guideViewGameObject)
        {
            int curGameObjectIndex = guideViewGameObject.transform.GetSiblingIndex();
            int nextGameObjectIndex = curGameObjectIndex + 1;

            if (nextGameObjectIndex < guideViewGameObject.transform.parent.childCount)
            {
                var popUp = UIPopup.GetUIPopup("DoubleButtonPopup");

                popUp.SetDependencyOnView(UINavigation.CurrentView)
                    .SetButtonEvent(async () =>
                    {
                        await popUp.Hide();
                        UINavigation.PushUIView(guideViewGameObject.transform.parent.GetChild(nextGameObjectIndex).name);
                    })
                    .Show().Forget();
            }
            else
            {
                Debug.LogWarning("마지막 가이드 문서입니다.");
            }
        }
    }

}