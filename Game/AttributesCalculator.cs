using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributesCalculator : MonoBehaviour
{
    public const float SKILLDAMAGE = 33f;

    public const int EXPPERLEVEL = 25;

    public const float ARMORDMGRED = 50f;

    public const float BLUNTSKILLSTAGGERDIVISOR = 25f;

    public const float WEPWGTSTGRFCTR = 0.5F;

    public const float HPPERSTRENGTH = 10f;
    public const float STMPERAGILITY = 15f;
    public const float MANPERINTELLIGENCE = 15f;

    public const float BASEHEALTH = 10f;
    public const float BASESTAMINA = 15f;
    public const float BASEMANA = 15f;

    public const float BASESTAMINAREGEN = 0.5f;
    public const float STAMINAREGENPERAGILITY = 5f;
    
    public static float CalculateDamageMultiplier(int skill)
    {
        return skill / SKILLDAMAGE;
    }

    public static int GetExperienceByLevel(int level)
    {
        return level * EXPPERLEVEL;
    }
}
