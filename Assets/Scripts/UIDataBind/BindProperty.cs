using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UniRx.Triggers;
using UniRx;
using UnityEngine;

namespace uidatabind
{
    public enum UpdateDirection
    {
        Forward, Backward
    }

    public enum UpdateTime
    {
        OnUpdate, OnLateUpdate, OnFixedUpdate
    }

    public class BindProperty : MonoBehaviour
    {
        private PropertyInfo _targetProperty;
        private PropertyInfo _sourceProperty;

        [SerializeField] private Component _targetComponent;
        [SerializeField] private string _targetPropertyName;

        [SerializeField] private Component _sourceComponent;
        [SerializeField] private string _sourcePropertyName;

        [SerializeField] private UpdateDirection _updateDirection;
        [SerializeField] private UpdateTime _updateTime;

        public Component TargetComponent { get => _targetComponent; set => _targetComponent = value; }
        public Component SourceComponent { get => _sourceComponent; set => _sourceComponent = value; }
        public string TargetPropertyName { get => _targetPropertyName; set => _targetPropertyName = value; }
        public string SourcePropertyName { get => _sourcePropertyName; set => _sourcePropertyName = value; }
        public UpdateDirection UpdateDirection { get => _updateDirection; set => _updateDirection = value; }
        public UpdateTime UpdateTime { get => _updateTime; set => _updateTime = value; }

        private void Awake()
        {
            switch(_updateTime)
            {
                case UpdateTime.OnUpdate:
                    this.UpdateAsObservable()
                        .Subscribe(_ => UpdateProperty());
                    break;
                case UpdateTime.OnLateUpdate: 
                    this.LateUpdateAsObservable()
                        .Subscribe(_ => UpdateProperty());
                    break;
                case UpdateTime.OnFixedUpdate:
                    this.FixedUpdateAsObservable()
                        .Subscribe(_ => UpdateProperty());
                    break;
            }
        }

        /// <summary>
        /// Binding 대상 업데이트
        /// </summary>
        internal void UpdateProperty()
        {
            if (_targetPropertyName == null || _sourcePropertyName == null)
                return;

            // Target, Source Component, Property가 변경되었을 경우 캐싱
            if (_targetProperty == null || _sourceProperty == null || _targetPropertyName != _targetProperty.Name || _sourcePropertyName != _sourceProperty.Name)
            {
                _targetProperty = _targetComponent.GetType().GetProperty(_targetPropertyName);
                _sourceProperty = _sourceComponent.GetType().GetProperty(_sourcePropertyName);
            }

            // 업데이트 방향 지정
            // 정방향 경우, Target 값을 Source 값으로 업데이트.
            // 역방향 경우, Source 값을 Target 값으로 업데이트.
            switch (_updateDirection)
            {
                case UpdateDirection.Forward:
                    if (_targetProperty.PropertyType == typeof(string))
                        _targetProperty.SetValue(_targetComponent, _sourceProperty.GetValue(_sourceComponent, null).ToString(), null);
                    else
                        _targetProperty.SetValue(_targetComponent, _sourceProperty.GetValue(_sourceComponent, null), null);
                    break;
                case UpdateDirection.Backward:
                    if (_sourceProperty.PropertyType == typeof(string))
                        _sourceProperty.SetValue(_sourceComponent, _targetProperty.GetValue(_targetComponent, null).ToString(), null);
                    else
                        _sourceProperty.SetValue(_sourceComponent, _targetProperty.GetValue(_targetComponent, null), null);
                    break;
            }
        }
    }
}
