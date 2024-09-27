using Opsive.UltimateCharacterController.Traits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game
{
    public static class StatNames
    {
        static string[] attributes = {
            "Strength",
            "Agility",
            "Intelligence" };

        static string[] skills = {
            "Shield",
            "Blunt",
            "Sharp",
            "Ranged",
            "Stealth",
            "Alchemy",
            "Speech" };

        public static string GetAttributeName(Attribute attribute)
        {
            if ((int)attribute >= attributes.Length) return "UNNAMED ATTRIBUTE";
            return attributes[(int)attribute];
        }

        public static string GetSkillName(Skill skill)
        {
            if ((int)skill >= skills.Length) return "UNNAMED SKILL";
            return skills[(int)skill];
        }
    }
}
