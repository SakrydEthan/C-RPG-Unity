using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Attributes;

public class PlayerEquipmentController : MonoBehaviour
{
    public static PlayerEquipmentController instance;

    public PlayerAttributesController attributes;
    public PlayerHealthController health;
    public PlayerUIController ui;

    public ItemEquipable head;
    public ItemEquipable body;
    public ItemEquipable ring;
    public ItemEquipable necklace;
    public ItemEquipable[] equipment = new ItemEquipable[(int)EquipSlot.UNASSIGNED];

    public float[] resistance = new float[5];
    public List<SkillBonus> bonuses;


    // Start is called before the first frame update
    void Start()
    {
        if(instance == null) instance = this;
        attributes = GetComponent<PlayerAttributesController>();
        //bonuses = new List<SkillBonus>();
    }

    public void EquipItem(ItemEquipable item)
    {
        if(bonuses == null) bonuses = new List<SkillBonus>();
        if(attributes == null) attributes = GetComponent<PlayerAttributesController>();
        if(health == null) health = GetComponent<PlayerHealthController>();
        if(ui == null) ui = GetComponent<PlayerUIController>();
        if (item == null) return;

        RemoveItemBonuses(item.GetSlot());
        AddItemBonuses(item);
        equipment[(int)item.GetSlot()] = item;
        ui.EquipItem(item);
    }


    public void AddItemBonuses(EquipSlot slot)
    {
        if (equipment[(int)slot] == null) return;
        ItemEquipable item = equipment[(int)slot];
        for (int i = 0; i < item.resistance.Length; i++)
        {
            resistance[i] += item.resistance[i];
        }
        health.UpdateResistance(resistance);

        if (item.bonuses == null) return;
        for (int i = 0; i < item.bonuses.Length; i++)
        {
            bonuses.Add(item.bonuses[i]);
            attributes.AddSkillBonus(item.bonuses[i]);
        }
    }

    public void AddItemBonuses(ItemEquipable item)
    {
        if(item == null) return;
        for (int i = 0; i < item.resistance.Length; i++)
        {
            resistance[i] += item.resistance[i];
        }
        health.UpdateResistance(resistance);

        if (item.bonuses == null) return;
        for (int i = 0;i < item.bonuses.Length; i++)
        {
            bonuses.Add(item.bonuses[i]);
            attributes.AddSkillBonus(item.bonuses[i]);
        }
    }

    public void RemoveItemBonuses(EquipSlot slot)
    {
        if (equipment[(int)slot] == null) return;
        ItemEquipable item = equipment[(int)slot];
        for (int i = 0; i < item.resistance.Length; i++)
        {
            resistance[i] -= item.resistance[i];
        }

        if (item.bonuses == null) return;
        for (int i = 0; i < item.bonuses.Length; i++)
        {
            bonuses.Remove(item.bonuses[i]);
            attributes.RemoveSkillBonus(item.bonuses[i]);
            //if(bonuses.Count > 0) { }
        }
    }

    public void RemoveItemBonuses(ItemEquipable item)
    {
        if (item == null) return;
        for (int i = 0; i < item.resistance.Length; i++)
        {
            resistance[i] -= item.resistance[i];
        }

        if (item.bonuses == null) return;
        for (int i = 0; i < item.bonuses.Length; i++)
        {
            bonuses.Remove(item.bonuses[i]);
            attributes.RemoveSkillBonus(item.bonuses[i]);
            //if(bonuses.Count > 0) { }
        }
    }

    public ItemEquipable[] GetEquipment()
    {
        //ItemEquipable[] equipment = { head, body, ring, necklace };
        return equipment;
    }
}
