using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public abstract class UISection : MonoBehaviour
    {
        [SerializeField] protected GameObject panel;
        [SerializeField] protected bool isOpen;
        public virtual void OpenSection() { }
        public virtual void CloseSection() { }

        public virtual void ShowPanel() { panel.SetActive(true); }
        public virtual void HidePanel() { panel.SetActive(false); }
    }
}
