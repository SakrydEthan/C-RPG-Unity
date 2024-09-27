using Assets.Scripts.Items;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoot : UISection
{
    public static UILoot instance;
    public BaseCharacter character;
    //public List<Item> items;

    public Transform buttonParent;
    public GameObject itemButtonPrefab;
    public List<GameObject> buttonList;

    public float buttonOffsetStart = 15f;
    public float buttonMargin = 15f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static void LootCharacter(BaseCharacter character)
    {
        instance.character = character;
        instance.DisplayLootPanel();
    }

    public static void LootCharacter(List<Item> items)
    {
        instance.DisplayLootPanel(items);
    }

    public void DisplayLootPanel()
    {
        panel.SetActive(true);
        GenerateItemButtons();
        PlayerInstanceController.FreezePlayer();
    }

    public void DisplayLootPanel(List<Item> items)
    {
        panel.SetActive(true);
        GenerateItemButtons(items);
        PlayerInstanceController.FreezePlayer();
    }


    public void CloseLootPanel()
    {
        ClearItemButtons();
        character = null;
        panel.SetActive(false);
        PlayerInstanceController.UnfreezePlayer();
    }


    public void GenerateItemButtons()
    {
        //Debug.Log(inventory.items.Count);
        for (int i = 0; i < character.items.Count; i++)
        {
            //Debug.Log("i: " + i);
            GameObject instantiatedPrefab = Instantiate(itemButtonPrefab, buttonParent);

            float yPos = -buttonOffsetStart + (-i * buttonMargin);
            instantiatedPrefab.transform.localPosition = new Vector3(0f, yPos, 0f);
            instantiatedPrefab.GetComponent<ShopButton>().SetItem(character.items[i]);
            buttonList.Add(instantiatedPrefab);
        }
    }


    public void GenerateItemButtons(List<Item> items)
    {
        //Debug.Log(inventory.items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            //Debug.Log("i: " + i);
            GameObject instantiatedPrefab = Instantiate(itemButtonPrefab, buttonParent);

            float yPos = -buttonOffsetStart + (-i * buttonMargin);
            instantiatedPrefab.transform.localPosition = new Vector3(0f, yPos, 0f);
            instantiatedPrefab.GetComponent<ShopButton>().SetItem(items[i]);
            buttonList.Add(instantiatedPrefab);
        }
    }


    public void ClearItemButtons()
    {
        foreach (GameObject go in buttonList)
        {
            Destroy(go);
        }
        buttonList.Clear();
    }


}
