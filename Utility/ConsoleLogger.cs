using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleLogger : MonoBehaviour
{
    LogCategory[] logs;

    public void Start()
    {
        logs = new LogCategory[4];
        logs[0] = new LogCategory();
        logs[1] = new LogCategory();
        logs[2] = new LogCategory();
        logs[3] = new LogCategory();
    }

    public void LogMessage(string message, LogMessageType type)
    {
        logs[(int)type].LogMessage(Time.time.ToString()+message);
    }

    public List<string> GetLogByType(LogMessageType type)
    {
        return logs[(int)type].GetMessages();
    }
}

public class LogCategory
{
    List<string> logs;

    public LogCategory()
    {
        logs = new List<string>();
    }

    public void LogMessage(string message)
    {
        logs.Add(message);
    }

    public List<string> GetMessages()
        { return logs; }
}

public enum LogMessageType
{
    Game,
    Quest,
    Combat,
    Player
}
