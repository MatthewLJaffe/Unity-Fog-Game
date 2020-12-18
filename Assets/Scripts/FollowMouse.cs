using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowMouse : MonoBehaviour
{


    private Vector3 mouseWorld;


    void Update()
    {
        mouseWorld = Conversions.Instance.mousePosition();
        transform.position = Conversions.Instance.getGridSpace(new Vector3(mouseWorld.x + 1,mouseWorld.y+1,mouseWorld.z));
    }
}
