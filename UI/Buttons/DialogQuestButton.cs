using UnityEngine;
using System.Collections;
using TMPro;

public class DialogQuestButton : MonoBehaviour
{
    public Quest quest;
    

    public void SetQuest(Quest _quest)
    {
        quest = _quest;
        //GetComponentInChildren<TextMeshProUGUI>().text = quest.buttonTitle;
    }

    public void DisplayQuest()
    {
        //UIDialog.instance.DisplayQuest(quest);
    }
}
