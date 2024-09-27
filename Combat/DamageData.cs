using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    public class DamageData
    {
        public Transform source;
        public float amount;
        public float stagger;
        public DamageType type;

        public DamageData() { }

        public DamageData(Transform source, float amount, DamageType type, float stagger)
        {
            this.source = source;
            this.amount = amount;
            this.stagger = stagger;
            this.type = type;
        }

    }

    
}
