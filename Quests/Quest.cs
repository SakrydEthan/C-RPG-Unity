using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Create New/Quest", order = 0)]
public class Quest : ScriptableObject
{
    public int id;
    [SerializeField] Quest[] prerequisiteQuests;
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
            if (!QuestController.CheckQuestComplete(prerequisiteQuests[i])) return false;
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

        //Debug.Log("Quest is complete!");

        return isComplete;
    }

    public void CompleteQuest()
    {
        if (CheckComplete())
        {
            PlayerInventoryController.instance.AddItemsByArray(rewards);
            PlayerInventoryController.instance.gold += goldReward;
            QuestController.CompleteQuest(this);
        }
    }

    public string GetCompleteText()
    {
        bool isDone = CheckComplete();
        if (isDone) return completeText;
        else return uncompleteText;
    }
}
