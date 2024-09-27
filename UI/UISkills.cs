using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.UI;
using Opsive.UltimateCharacterController.Inventory;

public class UISkills : UISection
{

    public static UISkills instance;

    public GameObject background;

    public TextMeshProUGUI level;
    public TextMeshProUGUI statPoints;
    public TextMeshProUGUI attributePoints;

    public GameObject attributeButtonPrefab;
    public GameObject skillButtonPrefab;

    public Transform skillParent;
    public Transform attributeParent;
    public float buttonOffsetStart = 65f;
    public float buttonMargin = 45f;

    public List<SkillButton> skillList;
    public List<AttributeButton> attributeList;
    bool hasInstantiatedButtons = false;

    public TextMeshProUGUI shield;
    public TextMeshProUGUI blunt;
    public TextMeshProUGUI sharp;
    public TextMeshProUGUI ranged;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
    }

    public void ToggleSkillsPanel()
    {
        if (isOpen) CloseSkillsPanel();
        else OpenSkillsPanel();
    }

    public void OpenSkillsPanel()
    {
        panel.SetActive(true);
        if (background != null) background.SetActive(true);

        level.text = PlayerLevelController.instance.level.ToString();
        statPoints.text = "Stat Points Available: "+PlayerLevelController.instance.skillIncreasesAvailable.ToString();
        attributePoints.text = "Attribute Points Available: "+PlayerLevelController.instance.attributeIncreasesAvailable.ToString();

        //instantiate button lists if they are not instantiated
        if(!hasInstantiatedButtons)
        {
            GenerateAttributeButtons();
            GenerateSkillButtons();
            hasInstantiatedButtons = true;
        }

        foreach (SkillButton skill in skillList) skill.UpdateValue();
        foreach (AttributeButton attribute in attributeList) attribute.UpdateValue();

        isOpen = true;
        /*
        shield.text = PlayerAttributesController.instance.GetSkill(Skill.Shield).ToString();
        blunt.text = PlayerAttributesController.instance.GetSkill(Skill.Blunt).ToString();
        sharp.text = PlayerAttributesController.instance.GetSkill(Skill.Sharp).ToString();
        ranged.text = PlayerAttributesController.instance.GetSkill(Skill.Ranged).ToString();
        */
    }

    public void CloseSkillsPanel()
    {
        panel.SetActive(false);
        if (background != null) background.SetActive(false);
        isOpen = false;
    }

    public void UpdateValues()
    {
        statPoints.text = "Stat Points Available: " + PlayerLevelController.instance.skillIncreasesAvailable.ToString();
        attributePoints.text = "Attribute Points Available: " + PlayerLevelController.instance.attributeIncreasesAvailable.ToString();

        foreach (SkillButton skill in skillList) skill.UpdateValue();
        foreach (AttributeButton attribute in attributeList) attribute.UpdateValue();

        /*
        shield.text = PlayerAttributesController.instance.GetSkill(Skill.Shield).ToString();
        blunt.text = PlayerAttributesController.instance.GetSkill(Skill.Blunt).ToString();
        sharp.text = PlayerAttributesController.instance.GetSkill(Skill.Sharp).ToString();
        ranged.text = PlayerAttributesController.instance.GetSkill(Skill.Ranged).ToString();
        */
    }

    public static void IncreaseSkill(Skill skill)
    {
        PlayerAttributesController.instance.IncreaseSkill(1, skill);
    }


    public void GenerateSkillButtons()
    {
        Debug.Log("Creating " + (int)Skill.UNASSIGNED + " skill buttons");
        for (int i = 0; i < (int)Skill.UNASSIGNED; i++)
        {
            GameObject instantiatedPrefab = Instantiate(skillButtonPrefab, skillParent);

            float yPos = -buttonOffsetStart + (-i * buttonMargin);
            instantiatedPrefab.transform.localPosition = new Vector3(0f, yPos, 0f);
            instantiatedPrefab.GetComponent<SkillButton>().SetSkill((Skill)i);
            skillList.Add(instantiatedPrefab.GetComponent<SkillButton>());
        }
    }

    public void GenerateAttributeButtons()
    {
        for (int i = 0; i < (int)Attribute.UNASSIGNED; i++)
        {
            GameObject instantiatedPrefab = Instantiate(attributeButtonPrefab, attributeParent);

            float yPos = -buttonOffsetStart + (-i * buttonMargin);
            instantiatedPrefab.transform.localPosition = new Vector3(0f, yPos, 0f);
            instantiatedPrefab.GetComponent<AttributeButton>().SetAttribute((Attribute)i);
            attributeList.Add(instantiatedPrefab.GetComponent<AttributeButton>());
        }
    }
    public override void OpenSection()
    {
        OpenSkillsPanel();
    }

    public override void CloseSection()
    {
        CloseSkillsPanel();
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        if (background != null) background.SetActive(true);
    }

    public override void HidePanel()
    {
        base.HidePanel();
        if(background != null) background.SetActive(false);
    }
}
