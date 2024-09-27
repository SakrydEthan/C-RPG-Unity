using UnityEngine;
using System.Collections;
using TMPro;
using Assets.Scripts.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIInventory : UISection
    {

        public static UIInventory instance;
        public PlayerInventoryController inventory;
        public PlayerEquipmentController equipment;
        public Transform buttonParent;
        public RectTransform inventoryScrollField;

        public GameObject background;

        public GameObject inventoryButton;
        public float shiftRight = 120f;
        public float buttonOffsetStart = 65f;
        public float buttonMargin = 45f;

        List<GameObject> invButtons;

        public Item activeItem;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemDescription;

        public TextMeshProUGUI equipmentText;

        public Image[] equipped = new Image[(int)EquipSlot.UNASSIGNED];

        // Use this for initialization
        void Start()
        {
            invButtons = new List<GameObject>();
            if (instance == null)
            {
                instance = this; //PlayerInstanceController.instance.inventory;
                //DontDestroyOnLoad(gameObject);
                
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void DisplayInventory()
        {
            instance.panel.SetActive(true);
            if (instance.background != null) instance.background.SetActive(true);

            if (instance.inventory.items.Any() != true)
            {
                instance.SetBlankText();
            }
            else
            {
                instance.panel.SetActive(true);
                instance.UpdateItemText(instance.inventory.items[0]);
                instance.GenerateInventoryButtons();
                instance.UpdateEquipmentStats();
            }
        }

        public static void ToggleInventory()
        {
            if(!instance.isOpen)
            {
                instance.isOpen = true;
                //PlayerInstanceController.FreezePlayer();
                DisplayInventory();
            }
            else
            {
                instance.isOpen = false;
                //PlayerInstanceController.UnfreezePlayer();
                HideInventory();
            }
        }

        public static void HideInventory()
        {
            instance.ClearInventoryButtons();
            instance.panel.SetActive(false);
            if (instance.background != null) instance.background.SetActive(false);
        }


        public void GenerateInventoryButtons()
        {
            //Debug.Log(inventory.items.Count);
            for (int i = 0; i < inventory.items.Count; i++)
            {
                //Debug.Log("i: " + i);
                GameObject instantiatedPrefab = Instantiate(inventoryButton, buttonParent);

                float yPos = -buttonOffsetStart + (-i * buttonMargin);
                instantiatedPrefab.transform.localPosition = new Vector3(shiftRight, yPos, 0f);
                instantiatedPrefab.GetComponent<InventoryButton>().SetItem(inventory.items[i]);
                invButtons.Add(instantiatedPrefab);
            }
        }

        public void ClearInventoryButtons()
        {
            foreach(GameObject go in invButtons)
            {
                Destroy(go);
            }
            invButtons.Clear();
        }

        public void UseActiveItem()
        {
            if (activeItem == null) return;

            activeItem.UseItem();
            activeItem = null;
            ClearInventoryButtons();
            UpdateEquipmentStats();
            GenerateInventoryButtons();
        }

        public void UpdateActiveItem(Item item)
        {
            activeItem = item;
            UpdateItemText(item);
        }


        public void UpdateItemText(Item item)
        {
            itemName.text = item.itemName;
            itemDescription.text = item.itemDescription;

            if(item is ItemWeapon)
            {
                ItemWeapon weapon = (ItemWeapon)item;
                itemDescription.text += "\n\nSkill:" + EnumString.SkillToString(weapon.skill);
                itemDescription.text += "\nDamage Type: " + EnumString.DamageTypeToString(weapon.damageType);
                itemDescription.text += "\nDamage: " + PlayerInstanceController.GetWeaponDamage(weapon).ToString();
            }
            if(item is ItemEquipable)
            {
                ItemEquipable equipment = (ItemEquipable)item;
                bool hasAddedText = false;
                for (int i = 0; i < equipment.resistance.Length; i++)
                {
                    if (equipment.resistance[i] > 0)
                    {
                        if (!hasAddedText)
                        {
                            itemDescription.text += "\nResistance(s):";
                            hasAddedText = true;
                        }
                        itemDescription.text += "\n" + EnumString.DamageTypeToString((DamageType)i) + ": " + equipment.resistance[i];
                    }
                }
                if (equipment.bonuses == null) return;
                itemDescription.text += "\nBonus(es):";
                for (int i = 0; i < equipment.bonuses.Length; i++)
                {
                    itemDescription.text += EnumString.SkillToString(equipment.bonuses[i].skill) + ": " + equipment.bonuses[i].amount.ToString();
                }
            }
        }

        public void UpdateEquipmentStats()
        {
            equipment = PlayerInstanceController.instance.equipment;
            equipmentText.text = "";

            if (equipment.head) equipmentText.text += "Head: " + equipment.head.itemName;
            if (equipment.body) equipmentText.text += "\nBody: " + equipment.body.itemName;
            if (equipment.ring) equipmentText.text += "\nRing: " + equipment.ring.itemName;
            if (equipment.necklace) equipmentText.text += "\nNecklace: " + equipment.necklace.itemName;

            equipmentText.text += "\n\nResistances:";
            equipmentText.text += $"\nSlash: {equipment.resistance[0]}\nBlunt: {equipment.resistance[1]}" +
                $"\nPierce: {equipment.resistance[2]}\nMagic: {equipment.resistance[3]}\nPoison: {equipment.resistance[4]}\n";

            if(equipment.bonuses.Count > 0)
            {
                equipmentText.text += "\nBonuses: ";

                for (int i = 0; i < equipment.bonuses.Count; i++)
                {
                    equipmentText.text += "\n" + EnumString.SkillToString(equipment.bonuses[i].skill)+" +"+equipment.bonuses[i].amount;
                }
            }

        }

        public void SetBlankText()
        {
            itemName.text = "";
            itemDescription.text = "";
        }

        public override void OpenSection()
        {
            DisplayInventory();
        }

        public override void CloseSection()
        {
            HideInventory();
        }

        public override void ShowPanel()
        {
            base.ShowPanel();
            if (background != null) background.SetActive(true);
        }

        public override void HidePanel()
        {
            base.HidePanel();
            if (background != null) background.SetActive(false);
        }

        public void EquipItem(ItemEquipable item)
        {
            //if()
        }

    }

}