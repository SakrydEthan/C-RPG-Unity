using System;

[Serializable]
public class QuestObjective
{
    public bool isCompleted = false;

    public virtual bool CheckCompleted()
    {
        return isCompleted;
    }
}
