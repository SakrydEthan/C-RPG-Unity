using System;
using UnityEngine;

[Serializable]
public class QuestKillObjective : QuestObjective
{
    public string targetID = "";

    public override bool CheckCompleted()
    {
        bool targetDead = QuestController.CheckCharacterDead(targetID);
        string isDea = (targetDead) ? "dead" : "alive";
        Debug.Log("target: "+targetID+" is "+isDea);
        return targetDead;
    }
}
