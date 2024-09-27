

public static class EnumString
{
    public static string SkillToString(Skill skill)
    {
        switch (skill)
        {
            case Skill.Shield:
                return "Shield";
            case Skill.Blunt:
                return "Blunt";
            case Skill.Sharp:
                return "Sharp";
            case Skill.Ranged:
                return "Ranged";
            default:
                return "SKILL";
        }
    }

    public static string DamageTypeToString(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Slash:
                return "Slash";
            case DamageType.Blunt:
                return "Blunt";
            case DamageType.Pierce:
                return "Pierce";
            case DamageType.Magic:
                return "Magic";
            case DamageType.Poison:
                return "Poison";
            default:
                return "DAMAGE";
        }
    }
}