using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Dialog")]
public class CharacterDialog : ScriptableObject
{
    public DialogTopic[] topics;
    [TextArea]
    public string intro = "";
}

[Serializable]
public class DialogTopic
{
    [TextArea]
    public string title = "";
    [TextArea]
    public string dialog = "";
}