using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  

//#if UNITY_EDITOR

[CustomEditor(typeof(BaseCharacter), true)]
public class ObjectGUID : UnityEditor.Editor
{

    //private SerializedProperty ser;

    //private void OnEnable()
    //{
        //ser = serializedObject.FindProperty("id");
    //}

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //serializedObject.Update();

        //EditorGUILayout.PropertyField(ser);

        BaseCharacter character = (BaseCharacter)target;
        if(GUILayout.Button("Generate GUID"))
        {
            character.id = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(character);
        }

        //serializedObject.ApplyModifiedProperties();
    }
}

//#endif