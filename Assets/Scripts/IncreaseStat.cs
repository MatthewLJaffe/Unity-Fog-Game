using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStat : MonoBehaviour, Effect
{
    [SerializeField] private string type;
    [SerializeField] private int ammount;
    public delegate void forStatIncrease(GameObject piece);
    public static event forStatIncrease onStatIncrease;
    public void doEffect() 
    {
        GameObject activePiece = PieceList.Instance.getEngagedPiece();
        if(type.Equals("attack")) {
            activePiece.GetComponent<Character>().stats.addDamage += ammount;
        } 
        else if(type.Equals("move range")) {
            activePiece.GetComponent<Character>().stats.addMoveRange += ammount;
        }
        else if (type.Equals("attack range")) {
            activePiece.GetComponent<Character>().stats.addAttackRange += ammount;
        } 
        else if (type.Equals("uncover range")) {
            activePiece.GetComponent<Character>().stats.addUncoverRange += ammount;
        }
        onStatIncrease(activePiece);
    }
}
