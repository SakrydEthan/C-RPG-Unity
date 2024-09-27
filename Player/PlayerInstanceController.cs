using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Items;
using Assets.Scripts.Saving;
using Assets.Scripts.UI;
using UnityEngine;

public class PlayerInstanceController : MonoBehaviour
{

    public static PlayerInstanceController instance;
    public PlayerInteractionController interaction;
    public PlayerInventoryController inventory;
    public PlayerCombatController combat;
    public PlayerHealthController health;
    public PlayerAttributesController attributes;
    public PlayerEquipmentController equipment;
    public PlayerLevelController level;

    BaseCharacter conversant;
    bool isShopping = false;

    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(gameObject);


        if(instance == null)
        {
            AwakeBehavior();
            if (SaveController.CheckForSave()) SaveController.LoadPlayerSave();
            else NewGameController.ApplyChoicesToPlayer();

            /*
             * Loading a game from a save should be handled in the savecontroller,
             * and starting a new game should be handled by the newgamecontroller
             * 
             * 
            //if(SaveController.CheckForSave()) LoadPlayerSave(SaveController.instance.GetPlayerSave());
            if (SaveController.CheckForSave()) LoadPlayerSave();
            else 
            { 
                attributes.GetStartingTraits();
                for (int i = 0; i < NewGameController.instance.playerItems.Count; i++)
                {
                    inventory.AddItem(NewGameController.instance.playerItems[i]);
                }
            }
            */
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetPosition()
    {
        //Debug.Log("Setting player position to: " + SaveController.instance.startPosition.ToString());
        //transform.position = SaveController.instance.startPosition;

    }

    //public static void Resubscribe

    void AwakeBehavior()
    {
        instance = this;
        interaction = GetComponent<PlayerInteractionController>();
        inventory = GetComponent<PlayerInventoryController>();
        combat = GetComponent<PlayerCombatController>();
        health = GetComponent<PlayerHealthController>();
        attributes = GetComponent<PlayerAttributesController>();
        equipment = GetComponent<PlayerEquipmentController>();
        level = GetComponent<PlayerLevelController>();
        Debug.Log("setting ui instance of inventory");
        UIController.instance.SetPlayerInstance();
    }

    public static void EquipWeapon(ItemWeapon weapon) => instance.combat.EquipWeapon(weapon);

    public void SetPlayerPositionLoad(Vector3 position, Vector3 rotation)
    {
        transform.position = position;
        //camera is rotated in cameracontrols transform
        GetComponent<PlayerInteractionController>().camcon.transform.rotation = Quaternion.Euler(rotation);
    }

    //rewrite to allow other effects from consumables
    public static void UseConsumable(ItemConsumable consumable)
    {
        float heal = instance.health.hitpoints + consumable.amount;
        instance.health.hitpoints = Mathf.Clamp(heal, 0, instance.health.maxHitpoints);
        instance.inventory.items.Remove(consumable);
    }

    public static Vector3 GetRotation()
    {
        return instance.GetComponent<PlayerInteractionController>().camcon.transform.rotation.eulerAngles;
    }

    public static PlayerSave CreatePlayerSave()
    {

        PlayerSave playerSave = new PlayerSave(LevelController.GetActiveScene(),
            instance.transform.position, GetRotation(), instance.health.hitpoints, instance.attributes.GetStamina(),
            instance.attributes.GetMana(), instance.level.level, instance.level.exp,
            instance.level.skillIncreasesAvailable, instance.level.attributeIncreasesAvailable, 
            instance.attributes.GetSkills(), instance.attributes.GetAttributes(),
            instance.attributes.traits.ToArray(), instance.combat.rightItemWeapon, instance.equipment.GetEquipment(),
            instance.inventory.items, instance.inventory.gold, instance.inventory.totalWeight);

        return playerSave;
    }

    public void LoadPlayerSave()
    {
        PlayerSave playerSave = SaveController.GetPlayerSave();

        SetPlayerPositionLoad(playerSave.pos, playerSave.rot);
        attributes.LoadSavedAttributes(playerSave);
        //attributes.CreateFactors();

        if (playerSave.activeWeapon != null) combat.EquipWeapon(playerSave.activeWeapon);


        inventory.items = playerSave.inventory.GetAllItems();
        inventory.gold = playerSave.inventory.s_gold;
        inventory.totalWeight = playerSave.inventory.s_totalWeight;

        for (int i = 0; i < playerSave.equipment.Length; i++)
        {
            equipment.EquipItem(playerSave.equipment[i]);
        }

        level.StartBehavior(playerSave.xp, playerSave.level, playerSave.skillPoints);
        health.hitpoints = playerSave.hp;
    }


    //no longer used/needed
    public void LoadPlayerSave(PlayerSave save)
    {
        transform.position = save.pos;


        inventory.items = save.inventory.GetAllItems();
        inventory.gold = save.inventory.s_gold;
        inventory.totalWeight = save.inventory.s_totalWeight;
    }

    public static float GetWeaponDamage(ItemWeapon weapon) => instance.combat.GetWeaponDamage(weapon);

    public static void FreezePlayer() => instance.interaction.FreezePlayer();
    public static void UnfreezePlayer() => instance.interaction.UnfreezePlayer();

    public static bool CheckPlayerFrozen() { return instance.interaction.isFrozen; }

    public static void SetConversant(BaseCharacter character)
    {
        if(character is Merchant) UIShop.instance.merchant = (Merchant)character;
        instance.conversant = character;
        StartConversation();
    }

    public static void StartConversation()
    {
        Debug.Log("Player is now talking with "+instance.conversant.charName);
        instance.interaction.FreezePlayer();
    }

    public static void EndConversation()
    {
        if (instance.isShopping == true) return;
        if(instance.isShopping == false) instance.interaction.UnfreezePlayer();
    }

    public static void StartShopping()
    {
        Debug.Log("Start shopping was called");
        instance.isShopping = true;
        FreezePlayer();
    }

    public static void FinishShopping()
    {
        Debug.Log("Finish shopping was called");
        instance.isShopping = false;
        UnfreezePlayer();
    }
}
