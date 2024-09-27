using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.Scripts.Quests;

[CustomEditor(typeof(HouseDesigner), true)]
public class HouseDesignerUtility : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HouseDesigner designer = (HouseDesigner)target;
        if (GUILayout.Button("Generate Grid"))
        {
            designer.GenerateGrid();
        }
        if (GUILayout.Button("Generate House"))
        {
            designer.CreateHouse();
        }
        if (GUILayout.Button("Create Roof"))
        {
            designer.CreateRoof();
        }
        if (GUILayout.Button("Delete House"))
        {
            designer.DeleteGeneratedHouse();
        }
    }
}