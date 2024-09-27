using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.UI;

public class UIDialog : UISection
{
    public static UIDialog instance;
    public TextMeshProUGUI dialogText;

    public BaseCharacter character;

    public GameObject dialogButton;
    public Transform buttonParent;

    public GameObject shopButtonPrefab;
    public GameObject dialogButtonPrefab;
    public GameObject questButtonPrefab;

    int buttonIndex = 0;
    int dialogIndex = 0;
    int questIndex = 0;
    List<GameObject> dialogButtons;
    List<Quest> prereqUnmet;

    public float buttonOffsetStart = 10f;
    public float buttonMargin = 30f;



    QuestSO activeQuest = null;
    public GameObject acceptQuestButton;
    public GameObject declineQuestButton;

    public bool isTalking = false;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        prereqUnmet = new List<Quest>();
        dialogButtons = new List<GameObject>();
        dialogIndex = 0;
    }

    public void DisplayDialog(DialogOption dialog)
    {
        panel.SetActive(true);
        dialogText.text = "";
        dialogText.text += dialog.text;
        DisableQuestButtons();
    }

    //this was used for dialog by NPCs.
    //moved to dialog system, this is now obsolete
    //shop will be opened from dialog system
    public void StartDialog(BaseCharacter character)
    {
        if (isTalking == true) return;
        isTalking = true;

        PlayerInstanceController.FreezePlayer();
        character = this.character;
        panel.SetActive(true);
        dialogText.text = "";
        //dialogText.text += character.startingDialog[0];
        if (character is Merchant) GenerateShopButton();
        GenerateDialogButtons();
        GenerateQuestButtons();
    }

    public void ExitDialog()
    {
        //character = null;
        PlayerInstanceController.UnfreezePlayer();
        for (int i = 0; i < dialogButtons.Count; i++)
        {
            Destroy(dialogButtons[i]);
        }
        dialogButtons.Clear();
        prereqUnmet.Clear();

        panel.SetActive(false);
        isTalking = false;
        dialogIndex = 0;
    }

    public void UpdateDialogText(DialogOption dialog)
    {
        dialogText.text += "\n" + dialog.title + "\n" + dialog.text;
    }


    public void UpdateDialogText(DialogTopic topic)
    {
        dialogText.text += "\n" + topic.title + "\n" + topic.dialog;
    }

    public void SetDialogQuest(DialogQuestOption dialog)
    {
        activeQuest = dialog.quest;
        dialogText.text += activeQuest.questDescription;
        EnableQuestButtons();
    }

    public void DisplayQuest(QuestSO quest)
    {
        activeQuest = quest;
        dialogText.text += activeQuest.questDescription;
        EnableQuestButtons();
    }

    public void AcceptQuest()
    {
        //add active quest to inprogress quests on the questcontroller
        //QuestController.AddNewQuest(activeQuest);
        DisableQuestButtons();
        activeQuest = null;
    }

    public void DeclineQuest()
    {
        DisableQuestButtons();
        dialogText.text += activeQuest.declineText;
        activeQuest = null;
    }

    public void CompleteQuest()
    {
        if (activeQuest == null) return;
        //if (QuestController.CheckQuestComplete(activeQuest)) return; //dont allow player to repeatedly claim rewards
        DisableQuestButtons();
        dialogText.text += activeQuest.GetCompleteText();
        activeQuest.CompleteQuest();
        activeQuest = null;

        UpdateQuestButtons();
    }

    public void DeclineCompleteQuest()
    {
        if (activeQuest == null) return;
        DisableQuestButtons();
        dialogText.text += activeQuest.declineCompleteText;
        activeQuest = null;
    }

    public void EnableQuestButtons()
    {
        acceptQuestButton.SetActive(true);
        declineQuestButton.SetActive(true);
    }

    public void DisableQuestButtons()
    {
        acceptQuestButton.SetActive(false);
        declineQuestButton.SetActive(false);
    }

    public void GenerateShopButton()
    {
        Debug.Log("generating store button");
        GameObject instantiatedPrefab = Instantiate(shopButtonPrefab, buttonParent);

        float yPos = -buttonOffsetStart;
        instantiatedPrefab.transform.localPosition = new Vector3(0f, yPos, 0f);
        dialogButtons.Add(instantiatedPrefab);
        dialogIndex++;
    }

    public void GenerateDialogButtons()
    {
        //dialogIndex = 0;
        if (character.dialog == null) return;
        CharacterDialog dialog = character.dialog;

        for (int i = 0; i < dialog.topics.Length; i++)
        {
            //TODO convert to using characters dialog field instead of dialogbutton[]
            GameObject instantiatedPrefab = Instantiate(dialogButton, buttonParent);

            float yPos = -buttonOffsetStart + (-dialogIndex * buttonMargin);
            instantiatedPrefab.transform.localPosition = new Vector3(0f, yPos, 0f);
            instantiatedPrefab.GetComponent<DialogButton>().SetInteraction(dialog.topics[i]);
            dialogButtons.Add(instantiatedPrefab);
            dialogIndex++;
            //dialogButtons.Add(instantiatedPrefab);
        }

        /*
        for (int i = 0; i < character.startingDialog.Length; i++)
        {
            //TODO convert to using characters dialog field instead of dialogbutton[]
            GameObject instantiatedPrefab = Instantiate(dialogButton, buttonParent);

            float yPos = -buttonOffsetStart + (-i * buttonMargin);
            instantiatedPrefab.transform.localPosition = new Vector3(0f, yPos, 0f);
            instantiatedPrefab.GetComponent<DialogButton>().SetInteraction(character.startingDialog[i]);
            dialogButtons.Add(instantiatedPrefab);
            dialogIndex++;
            //dialogButtons.Add(instantiatedPrefab);
        }
        */
    }

    public void GenerateQuestButtons()
    {
        questIndex = 0;

        for (int i = 0; i < character.quests.Length; i++)
        {
            if (!character.quests[i].CheckPrerequisiteComplete())
            {
                Debug.Log("prereqs for " + character.quests[i].buttonTitle + " not met");
                //prereqUnmet.Add(character.quests[i]);
            }
            if (character.quests[i].CheckPrerequisiteComplete())
            {
                Debug.Log("prereqs for " + character.quests[i].buttonTitle + " are met");
                GameObject instantiatedPrefab = Instantiate(questButtonPrefab, buttonParent);

                float yPos = -buttonOffsetStart + (-(questIndex+dialogIndex) * buttonMargin);
                instantiatedPrefab.transform.localPosition = new Vector3(0f, yPos, 0f);
                //instantiatedPrefab.GetComponent<DialogQuestButton>().SetQuest(character.quests[i]);
                dialogButtons.Add(instantiatedPrefab);
                questIndex++;
            }
        }
    }

    public void AddQuestButton()
    {

    }

    public void UpdateQuestButtons()
    {
        //after completing a quest, add quests whose prereqs are now satisfied
        Debug.Log("checking if quests now meet prereqs");
        for (int i = 0; i < prereqUnmet.Count; i++)
        {
            /*
            if(prereqUnmet[i].CheckPrerequisiteComplete())
            {
                Debug.Log("prereqs for " + prereqUnmet[i].buttonTitle + " are now met");
                GameObject instantiatedPrefab = Instantiate(questButtonPrefab, buttonParent);

                float yPos = -buttonOffsetStart + (-(questIndex + dialogIndex) * buttonMargin);
                instantiatedPrefab.transform.localPosition = new Vector3(0f, yPos, 0f);
                instantiatedPrefab.GetComponent<DialogQuestButton>().SetQuest(prereqUnmet[i]);
                dialogButtons.Add(instantiatedPrefab);
                questIndex++;
            }
            */
        }
    }

}
