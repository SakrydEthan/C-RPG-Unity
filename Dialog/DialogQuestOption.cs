using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogOption", menuName = "Create New/Dialog/Quest", order = 2)]
public class DialogQuestOption : DialogOption
{

    public QuestSO quest;

    public override void Interaction()
    {
        UIDialog.instance.SetDialogQuest(this);
    }
}