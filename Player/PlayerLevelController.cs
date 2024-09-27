using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelController : MonoBehaviour
{

    public static PlayerLevelController instance;

    public int level = 0;
    public int exp = 0;
    [SerializeField] int[] expPerLvl = new int[99];

    public int basexpperlvl = 50;
    public int lvlmultifactor = 3;

    public int skillIncreasesPerLvl = 5;
    public int skillIncreasesAvailable = 0;

    public int attributeIncreasesPerLvl = 1;
    public int attributeIncreasesAvailable = 0;

    public float healthPerLevel = 5f;

    public void Start()
    {
        //TODO: move exp per level to dedicated class
        int expReq = 20;
        for (int i = 0; i < expPerLvl.Length; i++)
        {
            expPerLvl[i] = expReq;
            expReq = basexpperlvl*lvlmultifactor*i;
        }
        if (instance == null) instance = this;
        //AddExp(25);
    }

    public void AddExp(int _exp)
    {
        exp += _exp;
        Debug.Log("player gained " + _exp.ToString() + " exp, now has " + exp.ToString() + " total exp");
        int _newLevel = GetNewLevel(exp);
        if(level < _newLevel)
        {
            Debug.Log("Player leveled up!");
            skillIncreasesAvailable += skillIncreasesPerLvl * (_newLevel - level);
            attributeIncreasesAvailable += attributeIncreasesPerLvl * (_newLevel - level);
            int lvls = _newLevel - level;
            PlayerHealthController health = GetComponent<PlayerHealthController>();
            health.maxHitpoints += lvls * healthPerLevel;
            health.hitpoints += lvls * healthPerLevel;
            level = _newLevel;
        }
    }

    public void StartBehavior(int _exp, int _level, int skillIncreases)
    {
        PlayerHealthController health = GetComponent<PlayerHealthController>();

        health.maxHitpoints += healthPerLevel * (_level);

        level = _level;
        exp = _exp;
        skillIncreasesAvailable = skillIncreases;
    }

    public int GetNewLevel(int _exp)
    {
        int newLevel = 0;
        for (int i = 0; i < expPerLvl.Length; i++)
        {
            if (exp > expPerLvl[i]) newLevel++;
            //if(exp > expPerLvl[i-1] && exp <= expPerLvl[i]) { return i;  }
        }
        return newLevel;
    }

    public void LevelUp()
    {
    }
}
