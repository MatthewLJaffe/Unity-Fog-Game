using UnityEngine;

public class Heal : MonoBehaviour, Action<GameObject>
{
    public void doAction(GameObject reciever) 
    {
        reciever.GetComponent<Character>().health.changeHealth(GetComponent<Character>().stats.getDamage());
    }
}
