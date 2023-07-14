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
        public override void OnInspectorGUI()
        {
            BindProperty bindProperty = (BindProperty)serializedObject.targetObject;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_updateDirection"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_updateTime"));

            SerializedProperty source = serializedObject.FindProperty("_sourceComponent");
            SerializedProperty target = serializedObject.FindProperty("_targetComponent");

            EditorGUILayout.PropertyField(source);
            if (bindProperty.SourceComponent)
            {
                var propertyDesc = PropertiesInComponent(bindProperty.SourceComponent, bindProperty.SourcePropertyName);
                if (propertyDesc != null)
                {
                    bindProperty.SourceComponent = propertyDesc.Component;
                    bindProperty.SourcePropertyName = propertyDesc.Name;
                }
            }

            if ((UpdateDirection)(serializedObject.FindProperty("_updateDirection").enumValueIndex) == UpdateDirection.Forward)
                EditorGUILayout.LabelField("↓", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter});
            else
                EditorGUILayout.LabelField("↑", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });

            EditorGUILayout.PropertyField(target);
            if (bindProperty.TargetComponent)
            {
                var propertyDesc = PropertiesInComponent(bindProperty.TargetComponent, bindProperty.TargetPropertyName);
                if (propertyDesc != null)
                {
                    bindProperty.TargetComponent = propertyDesc.Component;
                    bindProperty.TargetPropertyName = propertyDesc.Name;
                }
            }

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