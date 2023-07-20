using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace uidatabind
{
    [CustomEditor(typeof(DataBindContext))]
    public class DataBindContextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DataBindContext context = (DataBindContext)target;

            var children = context.GetComponentsInChildren<IBindable>(true).ToList();

            EditorGUILayout.LabelField(UIDataBind.EDITOR_CONTEXT_INFO);
            EditorGUILayout.BeginVertical("Helpbox");
            {
                if (children != null && children.Count != 0)
                {
                    children.ForEach(item =>
                    {
                        DrawBindable(item);
                    });
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawBindable(IBindable bindable)
        {
            EditorGUILayout.BeginHorizontal("Helpbox");
            {
                if (bindable.GetType() == typeof(TextMeshProBinder))
                {
                    EditorGUILayout.LabelField((bindable as TextMeshProBinder).PreviewKey);
                    EditorGUILayout.LabelField(bindable.GetType().Name);
                }
                else
                {
                    EditorGUILayout.LabelField(bindable.Key);
                    EditorGUILayout.LabelField(bindable.GetType().Name);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
