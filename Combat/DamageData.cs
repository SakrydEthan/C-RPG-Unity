using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Combat
{
    public class DamageData
    {
        public float amount;
        public float stagger;
        public DamageType type;

        public DamageData() { }

        public DamageData(float amount, DamageType type, float stagger)
        {
            this.amount = amount;
            this.stagger = stagger;
            this.type = type;
        }

    }

    
}
