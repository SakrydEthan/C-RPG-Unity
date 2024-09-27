using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogOption", menuName = "Create New/Dialog/Option", order = 1)]
public class DialogOption : ScriptableObject
{
    public string title;
    [TextArea]
    public string text;

    public virtual void Interaction()
    {
        UIDialog.instance.UpdateDialogText(this);
    }

}
