using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogButton : MonoBehaviour
{
    [SerializeField] DialogOption option;
    [SerializeField] DialogTopic topic;
    [SerializeField] TextMeshProUGUI text;

    public void SetInteraction(DialogOption _option)
    {
        option = _option;
        text.text = _option.text;
        //GetComponentInChildren<TextMeshProUGUI>().text = this.option.title;
    }


    public void SetInteraction(DialogTopic _topic)
    {
        //option = _option;
        topic = _topic;
        text.text = topic.title;
        //GetComponentInChildren<TextMeshProUGUI>().text = this.option.title;
    }

    //public void Interact() => option.Interaction();

    public void Interact()
    {
        UIDialog.instance.UpdateDialogText(topic);
    }

}
