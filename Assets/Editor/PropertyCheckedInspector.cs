using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PropertyChecker
{

    [CustomEditor(typeof(MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public class PropertyCheckedEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            if (IsEnabledForObject())
            {
                DrawPropertyCheckerHeader();
                DrawInspector();
            }
            else
            {
                base.DrawDefaultInspector();
            }
        }

        bool IsEnabledForObject()
        {
            var type = PrefabUtility.GetPrefabType(this.target);
            return type == PrefabType.None || type == PrefabType.PrefabInstance || type == PrefabType.DisconnectedPrefabInstance;
        }

        //[MenuItem("Window/Utilities/Property Checker/Check Properties")]
        void DrawPropertyCheckerHeader()
        {
            var style = new GUIStyle(EditorStyles.label);
            style.fontSize = 7;
            style.normal.textColor = Color.gray;
            GUILayout.Label("Property checker enabled.", style);
        }

        void DrawInspector()
        {
            var obj = base.serializedObject;

            EditorGUI.BeginChangeCheck();
            obj.Update();
            SerializedProperty iterator = obj.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                {
                    DrawProperty(this.target.GetType(), iterator, this.targets.Select(a => (MonoBehaviour)a).ToArray());
                }
                enterChildren = false;
            }
            obj.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }

        void DrawProperty(System.Type componentType, SerializedProperty property, MonoBehaviour[] instances)
        {
            var propertyInfo = PropertyChecker.GetPropertyInfo(componentType, property, instances);
            if (propertyInfo.IsOptional)
            {
                DrawOptionalProperty(property);
            }
            else
            {
                if (propertyInfo.HasAssignedValue)
                {
                    DrawPropertyRequiredValueAssigned(property);
                }
                else
                {
                    DrawPropertyRequiredValueMissing(property);
                }
            }
        }

        void DrawOptionalProperty(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property, true, new GUILayoutOption[0]);
        }

        void DrawPropertyRequiredValueMissing(SerializedProperty property)
        {
            var col = GUI.color;
            GUI.color = Color.red;
            EditorGUILayout.PropertyField(property, true, new GUILayoutOption[0]);
            GUI.color = col;
        }

        void DrawPropertyRequiredValueAssigned(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property, true, new GUILayoutOption[0]);
        }
    }
}