using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Saving;

public class QuestController : MonoBehaviour
{

    public static QuestController instance;

    public List<Quest> activeQuests;
    public List<Quest> completedQuests;
    public List<Quest> failedQuests;

    [SerializeReference] QuestInfoSave save;

    // Use this for initialization
    void Start()
    {
        if(instance == null)
        {
            instance = this;

            //load save data
            save = SaveController.GetQuestSave();
            if (save == null) return;

            activeQuests = save.inprogress;
            completedQuests = save.completed;
            failedQuests = save.failed;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static QuestInfoSave GetQuestSave()
    {
        if(instance.save == null)
        {
            instance.save = new QuestInfoSave(instance.completedQuests, instance.activeQuests, instance.failedQuests);
        }
        return instance.save;
    }

    public static bool CheckQuestActive(Quest quest)
    {
        return instance.activeQuests.Contains(quest);
    }

    public static bool CheckQuestComplete(Quest quest)
    {
        return instance.completedQuests.Contains(quest);
    }

    public static bool CheckQuestFailed(Quest quest)
    {
        return instance.failedQuests.Contains(quest);
    }

    public static void AddNewQuest(Quest quest)
    {
        instance.activeQuests.Add(quest);
    }

    public static void CompleteQuest(Quest quest)
    {
        instance.activeQuests.Remove(quest);
        instance.completedQuests.Add(quest);
    }

    public static void FailQuest(Quest quest)
    {
        instance.activeQuests.Remove(quest);
        instance.failedQuests.Add(quest);
    }

    public static bool CheckCharacterDead(string id)
    {
        BaseCharacterSave save = (BaseCharacterSave)SavePersistenceController.GetSaveObjectByID(id);
        if (save == null) save = (BaseCharacterSave)SaveController.GetSaveObjectByID(id);
        if (save == null) return false;
        return !save.IsAlive;
    }
}
