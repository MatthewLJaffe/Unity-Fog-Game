using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySelector : MonoBehaviour
{
    public static EntitySelector Instance { get; private set; }

    public static GameObject selected;
    private Vector2 boxVector;
    private void Awake() 
    {
        boxVector = new Vector2(1, 1);
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0) && Physics2D.OverlapBox(transform.position,boxVector,0,LayerMask.GetMask("Impassable"))!= null) 
        {
            if (Physics2D.OverlapBox(transform.position, boxVector, 0,LayerMask.GetMask("Impassable")).gameObject.GetComponent<Character>() != null) {
                selected = Physics2D.OverlapBox(transform.position, boxVector, 0,LayerMask.GetMask("Impassable")).gameObject;
            }          
        }
        if(Input.GetMouseButtonUp(0)) {
            StartCoroutine(unselect());
        }
    }
    private IEnumerator unselect() {
        yield return new WaitForSeconds(.1f);
        selected = null;
    }
}
