using UnityEngine;
using System.Collections;
using UnityEditor;

public class Quest
{
    public string guid;
    public QuestStatus status;
    public Quest(string guid, QuestStatus status)
    {
        this.guid = guid;
        this.status = status;
    }

    public string GetGuid() { return guid; }
    public QuestStatus GetStatus() { return status; }

    public void SetStatus(QuestStatus status) {  this.status = status; }
}
