
using UnityEngine;
using UnityEditor;
using Assets.Scripts.Quests;

[CustomEditor(typeof(QuestDatabaseSO), true)]
public class CreateQuestDB : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestDatabaseSO db = (QuestDatabaseSO)target;
        if (GUILayout.Button("Refresh Quests"))
        {
            Debug.Log("Refreshing quest database");
            db.ClearDatabase();


            string[] guids;

            guids = AssetDatabase.FindAssets("t:QuestSO");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Debug.Log("Quest: " + assetPath);
                QuestSO quest = (QuestSO)AssetDatabase.LoadAssetAtPath(assetPath, typeof(QuestSO));
                //QuestSO quest = Resources.Load<QuestSO>(guid);
                if(quest != null) db.AddQuest(quest);
            }
            //QuestSO[] quests = AssetDatabase.FindAssets("t:QuestSO");
            EditorUtility.SetDirty(db);
        }
    }
}