using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Saving;
using PixelCrushers.DialogueSystem;
using System.Linq;
using Assets.Scripts.Quests;

public class QuestController : MonoBehaviour
{

    public static QuestController instance;

    public Dictionary<string, Quest> quests = new Dictionary<string, Quest>();
    [SerializeReference] public List<Quest> questsList;
    //public List<QuestSO> activeQuests;
    //public List<QuestSO> completedQuests;
    //public List<QuestSO> failedQuests;

    [SerializeReference] QuestInfoSave save;

    public QuestDatabaseSO questDatabase;
    Dictionary<string, QuestSO> questDatabaseDictionary = new Dictionary<string, QuestSO>();

    // Use this for initialization
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            questDatabaseDictionary = questDatabase.CreateQuestDictionary();

            //load save data (HANDLED BY SAVE CONTROLLER)
            //save = SaveController.GetQuestSave();
            //if (save != null) LoadQuestSave(save);
        }
    }

    public static QuestInfoSave GetQuestSave()
    {
        if(instance.save == null)
        {
            //instance.save = new QuestInfoSave(instance.completedQuests, instance.activeQuests, instance.failedQuests);
            //Quest[] questInfo = instance.quests.Values.ToArray();
            instance.save = new QuestInfoSave(instance.quests.Values.ToArray());
        }
        return instance.save;
    }

    public static void LoadQuestSave(QuestInfoSave save)
    {
        //instance.questDatabaseDictionary = new Dictionary<string, QuestSO>();
        instance.quests = new Dictionary<string, Quest>();
        foreach (Quest quest in save.quests)
        {
            Debug.Log($"Loading quest id:{quest.guid} status: {quest.status}");
            //instance.questDatabaseDictionary.Add(quest.guid, instance.questDatabase.GetQuestSOByGUID(quest.guid));
            instance.quests.Add(quest.guid, quest);
        }
    }

    public static bool CheckQuestActive(string guid)
    {
        //return instance.activeQuests.Contains(quest);
        if (guid == null) Debug.LogWarning("checked for active quest with blank guid");
        if (!instance.quests.ContainsKey(guid)) return false;
        return instance.quests[guid].GetStatus() == QuestStatus.Active;
    }

    public static bool CheckQuestComplete(string guid)
    {
        if (guid == null) Debug.LogWarning("checked for complete quest with blank guid");
        if (!instance.quests.ContainsKey(guid)) return false;
        Quest quest = instance.quests[guid];

        if (!instance.questDatabaseDictionary.ContainsKey(guid)) { Debug.Log($"Quest {guid} not found in quest dictionary!"); }
        else { 
            QuestSO so = instance.questDatabaseDictionary[guid];

            bool complete = so.CheckComplete();
            if (complete)
            {
                //if(quest.GetStatus() == QuestStatus.Failed) return false
                if (quest.GetStatus() != QuestStatus.Complete)
                {//give player their reward for completing the quest
                    so.CompleteQuest();
                }
                quest.SetStatus(QuestStatus.Complete);
            }
            return complete;
        }

        return true;
        //return instance.quests[guid].GetStatus() == QuestStatus.Complete;
    }

    public static bool CheckQuestFailed(string quest)
    {
        //return instance.failedQuests.Contains(quest);
        if (!instance.quests.ContainsKey(quest)) return false;
        return instance.quests[quest].GetStatus() == QuestStatus.Failed;
    }

    public static bool CanOfferQuest(string guid)
    {
        if (instance.quests.ContainsKey(guid)) return false;
        else return true;
    }

    public static void AddNewQuest(string guid)
    {
        //instance.activeQuests.Add(quest);
        Debug.Log($"Added quest w/ guid:{guid} to active quests");
        Quest quest = new Quest(guid, QuestStatus.Active);
        instance.quests.Add(guid, quest);
        //instance.questsList.Add(quest);
    }

    public static void CompleteQuest(Quest quest)
    {
        //instance.activeQuests.Remove(quest);
        //instance.completedQuests.Add(quest);
        if (!instance.quests.ContainsKey(quest.GetGuid())) return;
        instance.quests[quest.GetGuid()].SetStatus(QuestStatus.Complete);
    }
    public static void CompleteQuest(string quest)
    {
        if (!instance.quests.ContainsKey(quest)) return;
        instance.quests[quest].SetStatus(QuestStatus.Complete);
    }

    public static void FailQuest(Quest quest)
    {
        //instance.activeQuests.Remove(quest);
        //instance.failedQuests.Add(quest);
    }

    public static bool CheckCharacterDead(string id)
    {
        BaseCharacterSave save = (BaseCharacterSave)SavePersistenceController.GetSaveObjectByID(id);
        if (save == null) save = (BaseCharacterSave)SaveController.GetSaveObjectByID(id);
        if (save == null) return false;
        return !save.IsAlive;
    }

    #region RegisterLUA
    void OnEnable()
    {
        Lua.RegisterFunction("CanOfferQuest", this, SymbolExtensions.GetMethodInfo(() => CanOfferQuest(string.Empty)));
        Lua.RegisterFunction("CheckQuestComplete", this, SymbolExtensions.GetMethodInfo (() => CheckQuestComplete(string.Empty)));
        Lua.RegisterFunction("CheckQuestActive", this, SymbolExtensions.GetMethodInfo(() => CheckQuestActive(string.Empty)));
        Lua.RegisterFunction("AddNewQuest", this, SymbolExtensions.GetMethodInfo(() => AddNewQuest(string.Empty)));
    }

    void OnDisable()
    {
        Lua.UnregisterFunction("CanOfferQuest");
        Lua.UnregisterFunction("CheckQuestComplete");
        Lua.UnregisterFunction("CheckQuestActive");
        Lua.UnregisterFunction("AddNewQuest");
    }
    #endregion
}
