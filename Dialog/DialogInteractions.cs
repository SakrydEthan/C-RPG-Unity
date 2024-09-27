using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractions : MonoBehaviour
{
    public static DialogInteractions instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void RestoreHealth(double amount)
    {
        Debug.Log($"Restoring {amount} to player's hp");
        //PlayerInstanceController.instance.attributes.
        PlayerInstanceController.instance.attributes.AddHealth((float)amount);
    }

    public void RestoreStamina(double amount)
    {
        PlayerInstanceController.instance.attributes.AddStamina((float)amount);
    }

    public void RestoreMana(double amount)
    {
        PlayerInstanceController.instance.attributes.AddMana((float)amount);
    }

    public bool CheckPlayerMoney(double amount)
    {
        return PlayerInstanceController.instance.inventory.HasGold(Mathf.RoundToInt((float)amount));
    }

    public void SpendPlayerMoney(double amount)
    {
        PlayerInstanceController.instance.inventory.SpendGold(Mathf.RoundToInt((float)amount));
    }

    public void AddPlayerMoney(double amount)
    {
        PlayerInstanceController.instance.inventory.AddGold(Mathf.RoundToInt((float)amount));
    }

    #region RegisterLUA
    void OnEnable()
    {
        Lua.RegisterFunction("RestoreHealth", this, SymbolExtensions.GetMethodInfo(() => RestoreHealth(0)));
        Lua.RegisterFunction("RestoreStamina", this, SymbolExtensions.GetMethodInfo(() => RestoreStamina(0)));
        Lua.RegisterFunction("RestoreMana", this, SymbolExtensions.GetMethodInfo(() => RestoreMana(0)));


        Lua.RegisterFunction("CheckPlayerMoney", this, SymbolExtensions.GetMethodInfo(() => CheckPlayerMoney(0)));
        Lua.RegisterFunction("SpendPlayerMoney", this, SymbolExtensions.GetMethodInfo(() => SpendPlayerMoney(0)));
        Lua.RegisterFunction("AddPlayerMoney", this, SymbolExtensions.GetMethodInfo(() => AddPlayerMoney(0)));
    }

    void OnDisable()
    {
        Lua.UnregisterFunction("RestoreHealth");
        Lua.UnregisterFunction("RestoreStamina");
        Lua.UnregisterFunction("RestoreMana");


        Lua.UnregisterFunction("CheckPlayerMoney");
        Lua.UnregisterFunction("SpendPlayerMoney");
        Lua.UnregisterFunction("AddPlayerMoney");
    }
    #endregion
}
