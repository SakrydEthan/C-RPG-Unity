using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {

        public static UIController instance;

        public UISection activeSection;
        public UISection[] sections;

        public UIInventory inventory;
        public UIDialog dialog;
        public UIShop shop;
        public UILoot loot;
        public UISkills skills;

        public TextMeshProUGUI interactText;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI staminaText;
        public TextMeshProUGUI manaText;

        public bool panelOpen = false;

        public Image hpBar;
        public Image staminaBar;
        public Image manaBar;


        // Start is called before the first frame update
        void Start()
        {
            if (instance == null)
            {
                instance = this;
                interactText.text = "";
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            for(int i=0; i<sections.Length; i++) sections[i].HidePanel();
        }

        public void SetPlayerInstance()
        {
            UIInventory.instance.inventory = PlayerInstanceController.instance.inventory;
        }

        public void OpenDialog(DialogOption dialog) => instance.dialog.DisplayDialog(dialog);
        
        public void PauseGame() => GameStateController.PauseGame();
        public void UnpauseGame() => GameStateController.UnpauseGame();
        public void QuitGame() => GameStateController.QuitGame();

        /// <summary>
        /// Returns true if the inventory was not open, and false if it was already open.
        /// </summary>
        /// <returns>True if previously closed, false if already open</returns>
        public static bool ToggleInventory()
        {
            if(instance.activeSection == null)
            {
                PlayerInstanceController.FreezePlayer();
                instance.OpenInventory();
                return true;
            }
            else
            {
                if(instance.activeSection is UIInventory)
                {
                    PlayerInstanceController.UnfreezePlayer();
                    instance.CloseActiveSection();
                    return false;
                }
                else
                {
                    instance.CloseActiveSection();
                    instance.OpenInventory();
                    return true;
                }
            }
        }
        
        /// <summary>
        /// Opens the skill panel if it was not already open, and closes it if it was open.
        /// </summary>
        /// <returns>True if previously closed, false if already open</returns>
        public static bool ToggleSkills()
        {
            if (instance.activeSection == null)
            {
                PlayerInstanceController.FreezePlayer();
                instance.OpenSkills();
                return true;
            }
            else
            {
                if (instance.activeSection is UISkills)
                {
                    PlayerInstanceController.UnfreezePlayer();
                    instance.CloseActiveSection();
                    return false;
                }
                else
                {
                    instance.CloseActiveSection();
                    instance.OpenSkills();
                    return true;
                }
            }
        }

        public void OpenInventory()
        {
            if (activeSection != null) activeSection.CloseSection();

            inventory.OpenSection();
            activeSection = inventory;
        }

        public void OpenSkills()
        {
            if (activeSection != null) activeSection.CloseSection();

            skills.OpenSection();
            activeSection = skills;
        }

        public void CloseAllPanels()
        {
            for(int i=0; i<sections.Length; i++)
            {
                sections[i].CloseSection();
            }
        }

        public void CloseActiveSection() { 
            if(instance.activeSection != null) activeSection.CloseSection();
            activeSection = null;
        }

        public void CloseUI()
        {
            CloseActiveSection();
            PlayerInstanceController.UnfreezePlayer();
        }

        public static void UpdateHealth(float val, float max)
        {
            if (instance.healthText != null)
            {
                instance.healthText.text = val.ToString() + " / " + max.ToString();
            }
            else
            {
                if (instance.hpBar != null) instance.hpBar.fillAmount = Mathf.Clamp(val / max, 0, 1);
            }
        }
        public static void UpdateStamina(float val, float max)
        {
            if (instance.staminaText != null)
            {
                instance.staminaText.text = val.ToString() + " / " + max.ToString();
            }
            if (instance.staminaBar != null)
            {
                //float factor = val / max;
                //Debug.Log("setting stamina bar to " + factor);
                instance.staminaBar.fillAmount = Mathf.Clamp(val / max, 0, 1);
            }
        }
        public static void UpdateMana(float val, float max)
        {
            if (instance.manaText != null)
            {
                instance.manaText.text = val.ToString() + " / " + max.ToString();
            }
            else
            {
                if (instance.manaBar != null) instance.manaBar.fillAmount = Mathf.Clamp(val / max, 0, 1);
            }
        }

        public static void OpenShop()
        {
            instance.shop.OpenShop();
            PlayerInstanceController.FreezePlayer();
        }

        public static void EquipItem(ItemEquipable item)
        {
            instance.inventory.EquipItem(item);
        }
    }
}