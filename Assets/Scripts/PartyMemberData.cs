using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Member Stats", menuName ="Member Stats")]
public class PartyMemberData : ScriptableObject
{
    public string pieceName;
    public int hp;
    [SerializeField] private int damage;
    [SerializeField] private int moveRange;
    [SerializeField] private int attackRange;
    [SerializeField] private int uncoverRange;
    public string side;

    public int addDamage = 0;
    public int addMoveRange = 0;
    public int addAttackRange = 0;
    public int addUncoverRange = 0;

    public void resetStats() {
        addDamage = 0;
        addMoveRange = 0;
        addAttackRange = 0;
        addUncoverRange = 0;
    }

    public int getMoveRange() {
        return moveRange + addMoveRange;
    }

    public int getAttackRange() {
        return attackRange + addAttackRange;
    }

    public int getDamage() {
        return damage + addDamage;
    }

    public int getUncoverRange() {
        return uncoverRange + addUncoverRange;
    }
}
