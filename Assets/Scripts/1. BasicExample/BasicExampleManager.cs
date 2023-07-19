using Cysharp.Threading.Tasks;
using UnityEngine;
using uinavigation;
using uinavigation.popup;
using example.uidatabind;

namespace example.uinavigation
{
    public class BasicExampleManager : MonoBehaviour
    {
        private BasicExampleDataBind _dataBindExampleInstance;
        private BasicExampleDataBind _dataBindExample
        {
            get
            {
                if (_dataBindExampleInstance == null)
                    _dataBindExampleInstance = FindObjectOfType<BasicExampleDataBind>();
                return _dataBindExampleInstance;
            }
        }

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

        private uidatabind.DataBindContext _bindContext;

        private void Awake()
        {
            _bindContext = GetComponent<uidatabind.DataBindContext>();
            
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

        private void BindExamples()
        {
            _bindContext["Txt_LastContentView"] = _dataBindExample.BindText;
            _bindContext["Btn_LastContentView"] = _dataBindExample.OnClickBindExampleButton;
            _bindContext["Img_LastContentView"] = _dataBindExample.BindSprite;
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
                        if (nextGameObjectIndex == (guideViewGameObject.transform.parent.childCount - 1))
                        {
                            BindExamples();
                        }

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