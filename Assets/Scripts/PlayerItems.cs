using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Player Items",menuName ="Player Items")]
public class PlayerItems : ScriptableObject
{
    public List<GameObject> items;
}
