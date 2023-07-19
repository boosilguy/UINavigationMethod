using UnityEngine;

namespace example.uidatabind
{
    public class BasicExampleDataBind : MonoBehaviour
    {
        [Header("UI Data Binding Example")]
        [SerializeField] private Sprite _bindSprite;
        [SerializeField] private string _bindText;

        public Sprite BindSprite => _bindSprite;
        public string BindText => _bindText;

        public void OnClickBindExampleButton()
        {
            Debug.Log("[BasicExample] 축하합니다! Binding된 이벤트가 발생하였습니다.");
        }
    }

}
