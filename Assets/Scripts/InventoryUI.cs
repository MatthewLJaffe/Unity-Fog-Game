using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] private PlayerItems p1Items;
    [SerializeField] private PlayerItems p2Items;
    private PlayerItems listForTurn;
    public Transform itemsParent;
    InventorySlot[] slots;

    private void Awake() {
        //update items in inventory right after inventory is enabled
        UIManager.OnInventorySelected += tryUpdatingUI;
        slots = GetComponentsInChildren<InventorySlot>();
    }

    private void tryUpdatingUI() 
    {
        setListForTurn();
        StartCoroutine(updateUI(listForTurn));
    }

    private IEnumerator updateUI(PlayerItems playerItems) 
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("update ui");
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < playerItems.items.Count && playerItems.items[i] != null) 
            {
                slots[i].addItem(playerItems.items[i]);
            } 
            else 
            {
                slots[i].clearItem();
            }
        }
    }

    public void useItem(int slotNumber) 
    {
        setListForTurn();
        if(listForTurn.items.Count > slotNumber && listForTurn.items[slotNumber] != null) 
        {
            listForTurn.items[slotNumber].GetComponentInChildren<ItemCaller>().callEffect();
            listForTurn.items[slotNumber] = null;
            slots[slotNumber].clearItem();
        }
    }

    private void setListForTurn() {
        if (TurnManager.CURRENT_TURN == 1) {
            listForTurn = p1Items;
        } else {
            listForTurn = p2Items;
        }
    }
}
