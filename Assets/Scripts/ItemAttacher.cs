using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAttacher : MonoBehaviour
{
    [SerializeField] private PlayerItems playerItems;
    [SerializeField] private string playerSide;

    private void Awake() {
        ItemMagnetize.OnItemRecieved += attachItem;
        playerItems.items.Clear();
    }
    private void OnDisable() {
        ItemMagnetize.OnItemRecieved -= attachItem;
    }

    private void attachItem(GameObject item, string side) 
    {
        if(side.Equals(playerSide))
        {
            playerItems.items.Add(item);
        }
    }
}