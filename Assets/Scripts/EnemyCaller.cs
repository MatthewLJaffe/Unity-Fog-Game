using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCaller : MonoBehaviour 
{
    [SerializeField] private GameObject player;
    public static List<EnemyController> Enemys;
    private List<EnemyController> activeEnemys;
    private TurnManager turn;
    public delegate void enemyTurnArgs(Vector3 pos);
    public static event enemyTurnArgs OnEnemyTurn;
    private bool turnUnused = true;


    private void Awake() {
        Enemys = new List<EnemyController>();
        activeEnemys = new List<EnemyController>();
        turn = player.GetComponent<TurnManager>();
    }

    void Update() 
    {
        if (TurnManager.CURRENT_TURN == 3 && turnUnused)
        {
            getActiveEnemys();
            if (activeEnemys.Count == 0) {
                turn.advanceTurn();
            }
            for (int i = 0; i < activeEnemys.Count; i++) 
            {
                StartCoroutine(callEnemyAction(i));
            }
            turnUnused = false;
        }
        if(TurnManager.CURRENT_TURN != 3) {
            turnUnused = true;
        }
    }

    private IEnumerator callEnemyAction(int index) 
    {
        yield return new WaitForSeconds(3 * index);
        //may want to understand if enemy is still hidden and pass in position of piece being attacked instead
        OnEnemyTurn(activeEnemys[index].transform.position);
        yield return new WaitForSeconds(1);
        StartCoroutine(activeEnemys[index].useTurn());
        if (index == activeEnemys.Count - 1) 
        {
            yield return new WaitForSeconds(1);
            turn.advanceTurn();
        }
    }
    private void getActiveEnemys()
    {
        activeEnemys.Clear();
        for(int i = 0; i < Enemys.Count; i++) 
        {
            if(Enemys[i].target != null || Enemys[i].getAttacking()) 
            {
                activeEnemys.Add(Enemys[i]);
            }
        }
    }
}
