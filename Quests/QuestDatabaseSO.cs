
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Create New/etc/QuestDB", order = 0)]
    public class QuestDatabaseSO : ScriptableObject
    {
        public string databaseName = "main";
        public List<QuestSO> questSOs = new List<QuestSO>();

        public void AddQuest(QuestSO quest)
        {
            if(quest.id == "" || quest.id == null)
            {
                Debug.Log("Quest has blank id, was rejected");
                return;
            }
            //quests.Add(quest.id, quest);
            questSOs.Add(quest);
        }

        public QuestSO GetQuestSOByGUID(string guid)
        {
            for (int i = 0; i < questSOs.Count; i++)
            {
                if (questSOs[i].id.Equals(guid)) return questSOs[i];
            }

            Debug.Log("Did not find quest! guid:" + guid);
            return null;
        }

        public void ClearDatabase()
        {
            questSOs.Clear();

            questSOs = new List<QuestSO>();   
        }

        public Dictionary<string, QuestSO> CreateQuestDictionary()
        {
            Dictionary<string, QuestSO> questDictionary = new Dictionary<string, QuestSO>();
            foreach(QuestSO quest in questSOs){
                questDictionary.Add(quest.id, quest);
            }
            return questDictionary;
        }
    }
}
