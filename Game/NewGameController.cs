using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Assets.Scripts.Combat;
using TMPro;
using Assets.Scripts.Items;
using Assets.Scripts.Attributes;
using Unity.VisualScripting;
using Assets.Scripts.Game;

public class NewGameController : MonoBehaviour
{
    public static NewGameController instance;
    public string startingScene;

    public bool isDoneCustomising = false;
    public GameObject customisationPanel;
    public GameObject mainMenuPanel;
    public List<Trait> playerTraits;
    public List<Item> playerItems;

    public TextMeshProUGUI traitDescription;

    public List<NewGameTraitCollection> traitCollections;
    public List<NewGameItemCollection> itemCollections;

    public StartingClass startingClass;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        instance.customisationPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void BeginCharacterCustomisation()
    {
        instance.customisationPanel.SetActive(true);
        instance.mainMenuPanel.SetActive(false);
    }

    public static void StartNewGame()
    {
        if(!CheckCustomizationComplete()) return;

        //add selected traits to trait list
        for (int i = 0; i < instance.traitCollections.Count; i++)
        {
            for (int j = 0; j < instance.traitCollections[i].traits.Count; j++)
            {
                instance.playerTraits.Add(instance.traitCollections[i].traits[j]);
            }
        }

        //add selected items to item list
        for (int i = 0; i < instance.itemCollections.Count; i++)
        {
            for (int j = 0; j < instance.itemCollections[i].items.Count; j++)
            {
                instance.playerItems.Add(instance.itemCollections[i].items[j]);
            }
        }

        //add starting class to trait list
        instance.playerTraits.Add(instance.startingClass);

        //add class starting items
        Item[] classItems = instance.startingClass.GetStartingItems();
        for(int i = 0; i < classItems.Length; i++)
        {
            instance.playerItems.Add(classItems[i]);
        }

        LevelController.StartNewGame(instance.startingScene);
        /*
        instance.playerTraits.Clear();
        instance.isDoneCustomising = true;
        for (int i = 0; i < instance.traitCollections.Count; i++)
        {
            if (!instance.traitCollections[i].CheckComplete()) instance.isDoneCustomising = false;
            for (int j = 0; j < instance.traitCollections[i].traits.Count; j++)
            {
                instance.playerTraits.Add(instance.traitCollections[i].traits[j]);
            }
        }
        for (int i = 0; i < instance.itemCollections.Count; i++)
        {
            if (!instance.itemCollections[i].CheckComplete()) instance.isDoneCustomising = false;
            for (int j = 0; j < instance.itemCollections[i].items.Count; j++)
            {
                instance.playerItems.Add(instance.itemCollections[i].items[j]);
            }
        }


        if (!instance.isDoneCustomising)
        {
            instance.playerTraits.Clear();
            instance.playerItems.Clear();
            return;
        }
        LevelController.StartNewGame(instance.startingScene);
        */
    }

    public static void AddTrait(Trait trait)
    {
        instance.playerTraits.Add(trait);
    }
    public static void RemoveTrait(Trait trait)
    {
        if (instance.playerTraits.Contains(trait)) instance.playerTraits.Remove(trait);
    }

    public static void AddItems(Item[] items)
    {
        for (int i = 0; i < instance.playerItems.Count; i++)
        {
            instance.playerItems.Add(items[i]);
        }
    }

    public static void RemoveItem(Item item)
    {
        instance.playerItems.Remove(item);
    }

    public static void RemoveItems(Item[] items)
    {
        for (int i = 0; i < instance.playerItems.Count; i++)
        {
            instance.playerItems.Remove(items[i]);
        }
    }

    public static void ClearItems()
    { instance.playerItems.Clear(); }

    public static string GetStartSceneName()
    {
        return instance.startingScene;
    }

    public static void UpdateDescriptionText(Trait trait)
    {
        string description = trait.GetTraitName();
            
        description += "\n"+trait.GetDescription()+"\n";

        foreach (SkillBonus bonus in trait.GetSkills())
        {
            description += StatNames.GetSkillName(bonus.skill) + " +" + bonus.amount.ToString();
        }
        instance.traitDescription.text = description;
    }

    public static void ClearDescriptionText()
    {
        instance.traitDescription.text = "";
    }

    public static void SetStartingClass(StartingClass startingClass) { instance.startingClass = startingClass; }

    public static bool CheckCustomizationComplete()
    {
        if(instance.startingClass == null) return false;
        for(int i=0; i<instance.traitCollections.Count; i++)
        {
            if (!instance.traitCollections[i].CheckComplete()) return false;
        }
        for (int i = 0; i < instance.itemCollections.Count; i++)
        {
            if (!instance.itemCollections[i].CheckComplete()) return false;
        }

        return true;
    }

    public static string SkillToString(Skill skill)
    {
        switch (skill)
        {
            case Skill.Shield:
                return "Shield";
            case Skill.Sharp:
                return "Sharp";
            case Skill.Blunt:
                return "Blunt";
            case Skill.Ranged:
                return "Ranged";
            default:
                return "SKILL";
        }
    }
}
