using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using Assets.Scripts.Quests;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Create New/Quest", order = 0)]
public class QuestSO : ScriptableObject
{
    public string id = Guid.NewGuid().ToString();
    [SerializeField] QuestSO[] prerequisiteQuests;
    [SerializeField] QuestKillObjective[] killObjectives;
    [SerializeField] QuestItemObjective[] itemObjectives;

    [SerializeField] Item[] rewards;
    [SerializeField] int goldReward;

    public string buttonTitle = "Is there something I can do for you?";

    [TextArea]
    public string questDescription = "Yes, there is.";

    public string acceptText = "Excellent, come back as soon as you complete it.";
    public string declineText = "Ok, come back when you're ready then.";

    public string uncompleteText = "You haven't done everything I asked";
    public string declineCompleteText = "Ok, come back when you are finished.";
    public string completeText = "Very well done traveller!";

    public bool CheckPrerequisiteComplete()
    {
        if (prerequisiteQuests.Length == 0) return true;
        for (int i = 0; i < prerequisiteQuests.Length; i++)
        {
            if (!QuestController.CheckQuestComplete(prerequisiteQuests[i].id)) return false;
        }

        return true;
    }

    public bool CheckComplete()
    {
        bool isComplete = true;

        for(int i=0; i<killObjectives.Length; i++)
        {
            if (!killObjectives[i].CheckCompleted()) return false;
        }

        List<Item> items = new List<Item>();
        bool hasItems = true;
        for (int i = 0; i < itemObjectives.Length; i++)
        {
            Item[] objectiveItems = itemObjectives[i].GetItems();
            for (int j = 0; j < objectiveItems.Length; j++)
            {
                if (PlayerInstanceController.instance.inventory.HasItem(objectiveItems[i])){
                    PlayerInstanceController.instance.inventory.RemoveItem(objectiveItems[i]);
                    items.Add(objectiveItems[i]);
                }
                else hasItems = false;
            }
        }
        if(!hasItems)
        {//if player doesn't have the needed items, return the items they did have from their inventory
            for (int i = 0; i < items.Count; i++)
            {
                PlayerInstanceController.instance.inventory.AddItem(items[i]);
            }
            return false;
        }

        //Debug.Log("Quest is complete!");

        return isComplete;
    }

    public void CompleteQuest()
    {
        if (CheckComplete())
        {
            PlayerInventoryController.instance.AddItemsByArray(rewards);
            PlayerInventoryController.instance.gold += goldReward;
            //QuestController.CompleteQuest(id);
        }
    }

    public string GetCompleteText()
    {
        bool isDone = CheckComplete();
        if (isDone) return completeText;
        else return uncompleteText;
    }
}
