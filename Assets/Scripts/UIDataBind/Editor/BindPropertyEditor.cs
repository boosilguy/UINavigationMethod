using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace uidatabind
{
    [CustomEditor(typeof(BindProperty))]
    public class BindPropertyEditor : Editor
    {
        private Texture2D _upwardArrow;
        private Texture2D _downwardArrow;
        private GUIStyle _arrowStyle;
        
        private void OnEnable()
        {
            _upwardArrow = Resources.Load<Texture2D>(UIDataBind.EDITOR_UPWARD_ARROW);
            _downwardArrow = Resources.Load<Texture2D>(UIDataBind.EDITOR_DOWNWARD_ARROW);

            _arrowStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                stretchWidth = true,
                fixedHeight = 25,
                margin = new RectOffset(0, 0, 15, 15)
            };
        }

        public override void OnInspectorGUI()
        {
            BindProperty bindProperty = (BindProperty)target;
            SerializedProperty updateDirectionProperty = serializedObject.FindProperty("_updateDirection");
            SerializedProperty updateTimeProperty = serializedObject.FindProperty("_updateTime");
            SerializedProperty sourceProperty = serializedObject.FindProperty("_sourceComponent");
            SerializedProperty targetProperty = serializedObject.FindProperty("_targetComponent");

            EditorGUILayout.PropertyField(updateDirectionProperty);
            EditorGUILayout.PropertyField(updateTimeProperty);

            EditorGUILayout.BeginVertical("helpbox");
            {
                EditorGUILayout.PropertyField(sourceProperty);
                if (bindProperty.SourceComponent)
                {
                    var propertyDesc = PropertiesInComponent(bindProperty.SourceComponent, bindProperty.SourcePropertyName);
                    if (propertyDesc != null)
                    {
                        bindProperty.SourceComponent = propertyDesc.Component;
                        bindProperty.SourcePropertyName = propertyDesc.Name;
                    }
                }

                DrawUpdateDirection((UpdateDirection)updateDirectionProperty.enumValueIndex);

                EditorGUILayout.PropertyField(targetProperty);
                if (bindProperty.TargetComponent)
                {
                    var propertyDesc = PropertiesInComponent(bindProperty.TargetComponent, bindProperty.TargetPropertyName);
                    if (propertyDesc != null)
                    {
                        bindProperty.TargetComponent = propertyDesc.Component;
                        bindProperty.TargetPropertyName = propertyDesc.Name;
                    }
                }
            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private PropertyDescription PropertiesInComponent(Component component, string selectedName)
        {
            var components = component.GetComponents<Component>().ToList();
            var propertyNames = new List<string>();
            var propertyDescriptions = new List<PropertyDescription>();
            var selectedIndex = -1;

            foreach (var item in components)
            {
                item.GetType().GetProperties().ToList().ForEach(property =>
                {
                    if (property.Name == selectedName) selectedIndex = propertyNames.Count;
                    propertyNames.Add($"{item.name}.{property.Name}");
                    propertyDescriptions.Add(new PropertyDescription(property.Name, item));
                });
            }

            var newSelected = EditorGUILayout.Popup("Property", selectedIndex, propertyNames.ToArray());

            if (newSelected != selectedIndex)
                return propertyDescriptions[newSelected];
            
            return null;
        }

        private void DrawUpdateDirection(UpdateDirection selected)
        {
            if (selected == UpdateDirection.Forward)
                GUILayout.Label(_downwardArrow, _arrowStyle);
            else
                GUILayout.Label(_upwardArrow, _arrowStyle);
        }

        private class PropertyDescription
        {
            internal string Name { get; set; }
            internal Component Component { get; set; }
            
            public PropertyDescription(string name, Component component) 
            { 
                this.Name = name;
                this.Component = component;
            }
        }
    }
}