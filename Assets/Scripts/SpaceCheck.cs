using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpaceCheck : MonoBehaviour
{
    public static SpaceCheck Instance { get; private set; }
    private Vector2 boxVector;

    private void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else {
            Destroy(gameObject);
        }
        boxVector = new Vector2(1, 1);
    }

    public GameObject getOccupied(Vector3 pos) {
        Collider2D collider = Physics2D.OverlapBox(pos, boxVector, 0, LayerMask.GetMask("Impassable", "Unspawnable"));
        if(collider != null) {
            return collider.gameObject;
        }
        return null;
    }
}
