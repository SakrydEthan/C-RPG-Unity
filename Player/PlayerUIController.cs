using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.UI;
using Assets.Scripts.Items;

public class PlayerUIController : MonoBehaviour
{

    public TextMeshProUGUI interactText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI magicaText;

    PlayerHealthController health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<PlayerHealthController>();

        SetText();

        //UpdateHealth(health.hitpoints, health.maxHitpoints);
        ClearInteractText();
    }

    public void SetText()
    {
        interactText = UIController.instance.interactText;
        healthText = UIController.instance.healthText;
        staminaText = UIController.instance.staminaText;
        magicaText = UIController.instance.manaText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInteractText(string text)
    {
        interactText.text = text;
    }

    public void ClearInteractText()
    {
        interactText.text = "";
    }

    public void UpdateHealth(float current, float max)
    {
        UIController.UpdateHealth(current, max);
        //int cur = (int)current;
        //int mx = (int)max;
        //healthText.text = cur.ToString()+" / "+mx.ToString();
    }

    public void UpdateStamina(float current, float max)
    {
        UIController.UpdateStamina(current, max);
        //int cur = (int)current;
        //int mx = (int)max;
        //staminaText.text = cur.ToString()+" / "+mx.ToString();
    }

    public void UpdateMana(float current, float max)
    {
        UIController.UpdateMana(current, max);
        //int cur = (int)current;
        //int mx = (int)max;
        //magicaText.text = cur.ToString() + " / " + mx.ToString();
    }

    public void EquipItem(ItemEquipable item)
    {
        UIController.EquipItem(item);
    }
}
