using System.Collections;
using System.Collections.Generic;
using uinavigation.uiview;
using UnityEngine;
using UnityEngine.UI;

namespace example.uinavigation
{
    public class BasicExampleView : UIView
    {
        [SerializeField] private Button _nextButton;

        protected override void Start()
        {
            base.Start();
            _nextButton.onClick.AddListener(() => BasicExampleManager.Instance.OnClickNextButton(gameObject));
        }
    }
}
