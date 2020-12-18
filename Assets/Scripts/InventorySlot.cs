using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private GameObject item;
    public Image icon;

    public void addItem(GameObject newItem) {
        item = newItem;
        icon.sprite = newItem.GetComponent<ItemCaller>().itemStats.itemImage;
        Debug.Log("adding item");
        icon.enabled = true;
    }

    public void clearItem() {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
