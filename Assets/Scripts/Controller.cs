using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    protected Character character;



    protected bool inRangeSelected(Vector3 point, float range) {
        return Mathf.Abs(point.x - transform.position.x) < range + .5
            && Mathf.Abs(point.y - transform.position.y) < range + .5
            && ScreenBounds.inScreenBounds(point);
    }

    protected bool mustAttack() {
        for (int i = 0; i < PieceList.Instance.pieces.Count; i++) {
            if (inRangeSelected(PieceList.Instance.pieces[i].transform.position, 1) && !PieceList.Instance.pieces[i].GetComponent<PlayerController>().character.stats.side.Equals(character.stats.side)) 
            {
                return true;
            }
        }
        return false;
    }
}
