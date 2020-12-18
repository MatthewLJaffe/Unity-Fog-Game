using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, Action<GameObject>
{
    public delegate void attackArgs(GameObject attacked, GameObject attacker);
    public static event attackArgs onAttack;
    public delegate void loseArgs();
    public static event loseArgs OnLose = delegate { };
    [SerializeField] private WinData winData;

    public void doAction(GameObject attacking)
    {
        Character attackedChar = attacking.GetComponent<Character>();
        attackedChar.health.changeHealth(-GetComponent<Character>().stats.getDamage());
        onAttack(attacking, gameObject);
        if (attackedChar.health.currentHealth <= 0) 
        {
            EnemyDropper enemyDropper = attacking.GetComponent<EnemyDropper>();
            if (enemyDropper != null) {
                enemyDropper.destroyer = gameObject;
            }
            attacking.SetActive(false);
            if (attacking.CompareTag("Player")) 
            {
                PieceList.Instance.pieces.Remove(attacking);
                if(PieceList.Instance.playersOnSide("Blue") == 0 ) {
                    winData.loser = "Blue";
                    OnLose();
                }
                else if(PieceList.Instance.playersOnSide("Red") == 0) {
                    winData.loser = "Red";
                    OnLose();
                }
            }
            else 
            {
                EnemyCaller.Enemys.Remove(attacking.GetComponent<EnemyController>());
            }
        }
    }
}