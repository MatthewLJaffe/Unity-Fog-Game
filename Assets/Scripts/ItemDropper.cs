using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private GameObject[] items;
    public delegate void ItemEnableArgs(GameObject recievingPiece);
    public static event ItemEnableArgs OnItemEnable;

    protected void InstantiateItem(Transform itemTrans, GameObject recipient) {
        Debug.Log("instantiate");
        Vector3 pos = itemTrans.position;
        Debug.Log(pos);
        int itemIndex = Random.Range(0, items.Length);
        Instantiate(items[itemIndex], pos, Quaternion.identity);
        OnItemEnable(recipient);
    }
}
