using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Items;
using Assets.Scripts.Character;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.Saving;
using Assets.Scripts.Combat;
using UnityEditor;
using PixelCrushers;
//using PixelCrushers.DialogueSystem;

public class BaseCharacter : MonoBehaviour, IInteractive, ISaveable, ICombatant
{
    public bool isAlive = true;
    public string id;
    public string charName;
    public List<Item> items;

    public QuestSO[] quests;
    //public DialogOption[] startingDialog;
    public CharacterDialog dialog;
    public AttributesController attributes;
    public NavMeshAgent agent;

    public HealthCharacter health;
    public float attackRange = 2f;
    public Transform target;
    public bool isAwake = false;
    public Weapon weapon;
    public GameObject weaponGO;
    protected ItemWeapon itemWeapon;
    public Transform rightHand;
    public Transform leftHand;
    //public 

    public Animator animator;

    protected bool isUnchanged = false;

    public int level = 1;
    public int killExp = 10;

    public CharacterPreset preset;

    public bool isAttacking = false;



    // Start is called before the first frame update
    void Start()
    {
        StartBehavior();
    }

    public virtual void StartBehavior()
    {
        Debug.Log("base character start behavior");
        SaveController.instance.objectSaveDelegate += Save;
        health = GetComponent<HealthCharacter>();
        attributes = GetComponent<AttributesController>();
        if (preset) SetPreset();
        Load();
    }


    public string GetInteractText()
    {
        if (!isAlive) return "Loot " + charName;
        return "Talk to " + charName;
    }

    public virtual void Interact()
    {
        if(!isAlive)
        {
            //open their inventory
            UILoot.LootCharacter(this);
            return;
        }

        //start conversation if char is alive and friendly
        Debug.Log("player interacted with " + charName);
        //UIController.instance.OpenDialog(startingDialog[0]);
        //UIDialog.instance.StartDialog(this);
        GetComponent<PixelCrushers.DialogueSystem.DialogueSystemTrigger>().OnUse();
        PlayerInstanceController.FreezePlayer();
        //nothing if char is aggressive and player not sneaking


        //open inventory if player is sneaking (pickpocket)


        //if(Player.IsSneaking()) open inventory
    }

    //used by dialogue system event to unfreeze player
    public void StartConversation()
    {
        PlayerInstanceController.SetConversant(this);
        PlayerInstanceController.StartConversation();
    }
    public void EndConversation()
    {
        PlayerInstanceController.EndConversation();
    }

    public virtual void Load()
    {
        //BaseCharacterSave save = (BaseCharacterSave)ES3.Load(id, Application.dataPath + SaveController.instance.saveName);
        if (id == null) return;
        if (GetComponent<SpawnedCharacter>() != null) return;

        BaseCharacterSave load = (BaseCharacterSave)SavePersistenceController.GetSaveObjectByID(id);
        if(load == null) load = (BaseCharacterSave)SaveController.GetSaveObjectByID(id);
        if (load == null) return;
        if (!load.IsAlive)
        {
            Destroy(gameObject);
            return;
        }

        health.hitpoints = load.Health;
    }

    public virtual void Save()
    {
        if (isUnchanged || id == null) return;

        if(GetComponent<SpawnedCharacter>() != null) return;

        BaseCharacterSave save = new BaseCharacterSave(id, isAlive);
        SavePersistenceController.instance.AddSaveObject(save);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void UpdateBehavior()
    { }

    //exp awarded for killing this character
    public virtual void GetKillExp()
    {
        PlayerLevelController.instance.AddExp(AttributesCalculator.GetExperienceByLevel(level));
    }

    public virtual void GetAttacked(Transform source)
    {
        if (!isAlive) return;
        if (target != null) return;
        Debug.Log(transform.name + " was attacked by " + source.name);
        isAwake = true;
        target = source;
        agent.SetDestination(source.position);
    }

    public virtual void GetStaggered()
    {
        if (!isAlive) return;
        animator.SetTrigger("stagger");
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    public virtual void GetStaggeredLight()
    {
        if (!isAlive) return;
        animator.SetTrigger("staggerLight");
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    public virtual void GetStaggeredMedium()
    {
        if (!isAlive) return;
        animator.SetTrigger("staggerMedium");
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    public virtual void GetStaggeredHeavy()
    {
        if (!isAlive) return;
        animator.SetTrigger("staggerHeavy");
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
    }

    public virtual void GetBlocked()
    {
        if(!isAlive) return;
        animator.SetTrigger("blocked");
        Debug.Log(charName + "'s attack was blocked");
    }


    public virtual void GetKilled()
    {
        if(!isAlive) return;
        //animator.SetTrigger("death");
        animator.SetTrigger("death");
        animator.SetBool("isDead", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);
        agent.isStopped = true;
        isAlive = false;
        
        GetKillExp();

        Debug.Log(charName + " was killed");
        BaseCharacterSave save = new BaseCharacterSave(id, false);
        if (GetComponent<SpawnedCharacter>() != null) { 
        }
        //SavePersistenceController.instance.AddSaveObject(save);

        //gameObject.SetActive(false);
    }

    public virtual void EquipWeapon(ItemWeapon _weapon)
    {
        //Debug.Log("character is equiping weapon: " + _weapon.itemName);
        itemWeapon = _weapon;
        //if (weaponGO != null) Destroy(weaponGO);
        //Debug.Log("Player equipped weapon: " + _weapon.itemName);
        //damage = attributes.GetWeaponDamage(_weapon);

        if (_weapon.isRightHanded) weaponGO = Instantiate(_weapon.model, rightHand);
        if (!_weapon.isRightHanded) weaponGO = Instantiate(_weapon.model, leftHand);
        weaponGO.transform.localPosition = _weapon.offset*100f;
        //weaponGO.transform.localRotation.SetEulerRotation(-90f, 0f, 0f);
        weaponGO.transform.Rotate(_weapon.rotation, Space.Self);
        weaponGO.transform.localScale = Vector3.one * 100f;

        if (weaponGO.GetComponent<Weapon>() == null)
        {
            Weapon _wep = weaponGO.AddComponent<Weapon>();
            _wep.SetItemWeapon(_weapon, GetComponent<AttributesController>());
            weapon = _wep;
            weapon.damage = attributes.GetWeaponDamage(_weapon);
        }
        else
        {
            weapon = weaponGO.GetComponent<Weapon>();
            weapon.damage = attributes.GetWeaponDamage(_weapon);
        }

        if(_weapon.overrideController != null) { animator.runtimeAnimatorController = _weapon.overrideController; }
    }

    public virtual void SetPreset()
    {
        if (preset == null) return;

        //Debug.Log("loading preset for " + charName);
        int[] preSkills = preset.GetSkills();
        for(int i = 0; i < preSkills.Length; i++)
        {
            attributes.SetSkill((Skill)i, preSkills[i]);
        }

        int[] preAttributes = preset.GetAttributes();
        for(int i = 0;i < preAttributes.Length; i++)
        {
            attributes.SetAttribute((Attribute)i, preAttributes[i]);
        }

        attributes.CreateFactors();
        if (weapon == null)
        {
            ItemWeapon _weapon = preset.GetWeapon();
            if(_weapon != null) EquipWeapon(_weapon);
        }
        //OLD METHOD
        /*
        if (preset == null) return;

        for (int i = 0; i < preset.skills.Length; i++)
        {
            if (attributes != null) attributes.SetSkill((Skill)i, preset.skills[i]);

        }

        if (weapon == null) EquipWeapon(preset.GetWeapon());

        for (int i = 0; i < preset.resistances.Length; i++)
        {
            health.resistance[i] = preset.resistances[i];
        }
        */
    }


    public virtual void AttackStart()
    {
        agent.isStopped = true;
        isAttacking = true;
        if(weapon != null) weapon.StartAttack();
    }

    public virtual void AttackEnd()
    {
        agent.isStopped = false;
        isAttacking = false;
        if (weapon != null) weapon.FinishAttack();
    }

    public virtual void ShootProjectile()
    {
        if (weapon != null) weapon.ShootWeapon();
    }

    private void OnValidate()
    {
        if(id == null || id == "" || id == " ")
        { 
            id = GUID.Generate().ToString();
            EditorUtility.SetDirty(this);
        }
    }
}
