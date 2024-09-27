using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine;
using TMPro;
using Assets.Scripts.UI;

public class UIShop : UISection
{
    public static UIShop instance;

    public TextMeshProUGUI shopGold;
    public TextMeshProUGUI playerGold;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;
    public TextMeshProUGUI itemCost;
    public Item activeItem;

    public GameObject shopButtonPrefab;
    public Transform buttonParent;
    public float buttonOffsetStart;
    public float buttonMargin;

    public Merchant merchant;

    List<GameObject> shopButtons = new List<GameObject>();


    private void Start()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }


    public void UpdateActiveItem(Item item)
    {
        activeItem = item;
        itemName.text = item.itemName;
        itemDesc.text = MakeItemDescription(item);
        itemCost.text = item.value.ToString()+"g";
    }

    public void UpdateShopText()
    {
        activeItem = null;
        itemName.text = "";
        itemDesc.text = "";
        itemCost.text = "";
        shopGold.text = "Shop gold: "+merchant.gold.ToString();
        playerGold.text = "Your gold: "+PlayerInstanceController.instance.inventory.gold.ToString();
    }


    public void BuyActiveItem()
    {
        if (activeItem == null) return;
        //PlayerInstanceController.instance.inventory.BuyItem(activeItem, activeItem.value);
        merchant.SellItem(activeItem);
        ClearShopButtons();
        GenerateShopButtons();
        UpdateShopText();
    }

    public void OpenShop()
    {
        Debug.Log("Open shop was called");
        if(merchant == null) return;
        itemName.text = "";
        shopGold.text = "Shop gold: " + merchant.gold.ToString();
        playerGold.text = "Your gold: " + PlayerInstanceController.instance.inventory.gold.ToString();
        panel.SetActive(true);
        GenerateShopButtons();
    }

    //public void 

    public void CloseShop()
    {
        activeItem = null;
        panel.SetActive(false);
        ClearShopButtons();
        merchant.CloseShop();
        PlayerInstanceController.FinishShopping();
    }

    public string MakeItemDescription(Item item)
    {
        string desc = "";
        desc += item.itemDescription;

        if (item is ItemWeapon)
        {
            ItemWeapon weapon = (ItemWeapon)item;
            desc += "\n\nSkill:" + EnumString.SkillToString(weapon.skill);
            desc += "\nDamage Type: " + EnumString.DamageTypeToString(weapon.damageType);
            desc += "\nDamage: " + PlayerInstanceController.GetWeaponDamage(weapon).ToString();
        }
        if (item is ItemEquipable)
        {
            ItemEquipable equipment = (ItemEquipable)item;
            bool hasAddedText = false;
            for (int i = 0; i < equipment.resistance.Length; i++)
            {
                if (equipment.resistance[i] > 0)
                {
                    if (!hasAddedText)
                    {
                        desc += "\nResistance(s):";
                        hasAddedText = true;
                    }
                    desc += "\n" + EnumString.DamageTypeToString((DamageType)i) + ": " + equipment.resistance[i];
                }
            }
            if (equipment.bonuses == null) return desc;
            desc += "\nBonus(es):";
            for (int i = 0; i < equipment.bonuses.Length; i++)
            {
                desc += EnumString.SkillToString(equipment.bonuses[i].skill) + ": " + equipment.bonuses[i].amount.ToString();
            }
        }

        return desc;
    }

    public void GenerateShopButtons()
    {
        //Debug.Log(inventory.items.Count);
        for (int i = 0; i < merchant.storeItems.Count; i++)
        {
            //Debug.Log("i: " + i);
            GameObject instantiatedPrefab = Instantiate(shopButtonPrefab, buttonParent);

            float yPos = -buttonOffsetStart + (-i * buttonMargin);
            instantiatedPrefab.transform.localPosition = new Vector3(0f, yPos, 0f);
            instantiatedPrefab.GetComponent<ShopButton>().SetItem(merchant.storeItems[i]);
            shopButtons.Add(instantiatedPrefab);
        }
    }


    public void ClearShopButtons()
    {
        foreach (GameObject go in shopButtons)
        {
            Destroy(go);
        }
        shopButtons.Clear();
    }



}
