using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item Stats", menuName ="Item Stats")]
public class ItemStats : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public string description;
}
