using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Items;
using Assets.Scripts.Saving;
using Assets.Scripts.UI;
using UnityEngine;

public class Merchant : BaseCharacter
{

    public List<Item> storeItems;
    public int gold = 0;
    public bool restockItems = false;

    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Called by DialogSystem so player can shop
    public void OpenMerchantShop()
    {
        UIController.OpenShop();
        PlayerInstanceController.FreezePlayer();
        PlayerInstanceController.StartShopping();
    }

    public void SellItem(Item item)
    {
        //Debug.Log("trying to sell: " + item.itemName);
        if (!storeItems.Contains(item)) return;
        if (!PlayerInstanceController.instance.inventory.TryBuyItem(item, item.value)) return;

        //Debug.Log("sold player " + item.itemName);
        PlayerInstanceController.instance.inventory.BuyItem(item, item.value);
        storeItems.Remove(item);
        gold += item.value;

        isUnchanged = false;
        /*


        //old method for saving merchant data
        
        MerchantSave starave = new MerchantSave(charName, transform.position, true, 100f, items, storeItems, gold);
        //MerchantSave starave = new MerchantSave();
        SavePersistenceController.instance.saveObjects.Add(starave);



        
         */
    }

    public void CloseShop()
    {
        MerchantSave save = new MerchantSave(id, transform.position, true, 100f, items, storeItems, gold);
        Debug.Log("saved character: " + charName + " with id: " + id);
        SavePersistenceController.instance.AddSaveObject(save);
    }

    //Should open dialog system conversation by default,
    //stealing menu if sneaking,
    //and loot menu if dead
    public override void Interact()
    {
        UIShop.instance.merchant = this;
        UIDialog.instance.character = this;
        //Debug.Log("player interacted with " + charName);
        //UIDialog.instance.StartDialog(this);
        GetComponent<PixelCrushers.DialogueSystem.DialogueSystemTrigger>().OnUse();
        PlayerInstanceController.FreezePlayer();
    }

    public override void Save()
    {
        MerchantSave starave = new MerchantSave(charName, transform.position, true, 100f, items, storeItems, gold);
        SavePersistenceController.instance.saveObjects.Add(starave);
    }

    public override void Load()
    {
        MerchantSave load = (MerchantSave)SavePersistenceController.GetSaveObjectByID(charName);
        if(load == null)
        {
            //no save data found in persistent data, check save file
            load = (MerchantSave)SaveController.GetSaveObjectByID(id);
        }
        if (load == null) return;
        if (!load.IsAlive) Destroy(gameObject);

        //after finding the latest save, apply its values
        if(!restockItems) items = load.Inventory.GetAllItems();
        storeItems = load.ShopItems.GetAllItems();
        gold = load.ShopItems.s_gold;
        //set position
        //set health
        //set rotation
    }
}
