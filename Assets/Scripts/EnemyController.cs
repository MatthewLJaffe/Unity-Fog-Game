using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyController : Controller
{
    public GameObject target;
    public delegate void ActionOn(GameObject go, bool set);
    public static event ActionOn changeMove = delegate { };
    public static event ActionOn changeAttack = delegate { };
    private Tilemap fog;
    private bool active = false;

    private void Awake() 
    {
        character = GetComponent<Character>();
        Attack.onAttack += setTarget;
        FogAway.OnFogRevealed += activate;
        fog = GameObject.FindGameObjectWithTag("Fog").GetComponent<Tilemap>();
    }
    private void Start() 
    {
        EnemyCaller.Enemys.Add(this);
    }
    private void OnDisable() {
        Attack.onAttack -= setTarget;
    }

    public IEnumerator useTurn() 
    {
        GameObject attacking = getAttacking();
        if(getAttacking() != null) 
        {
            changeAttack(gameObject, true);
            yield return new WaitForSeconds(1);
            character.action(attacking);
            changeAttack(gameObject, false);

        } 
        else if(target != null) 
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            moveDirection *= character.stats.getMoveRange();
            Debug.Log(transform.position + moveDirection);
            changeMove(gameObject, true);
            yield return new WaitForSeconds(1);
            changeMove(gameObject, false);
            StartCoroutine(character.moveTo(transform.position + moveDirection));
        }
    }

    private void activate(FogAway away) {
        if (fog.GetTile(Conversions.Instance.getGridSpace(transform.position)) == null && !active) {
            transform.GetChild(1).gameObject.SetActive(true);
            active = true;
            if(away.gameObject != gameObject) {
                target = away.gameObject;
            }           
        }
    }

    public GameObject getAttacking() 
    {
        List<GameObject> pList = PieceList.Instance.pieces;
        GameObject attacking = null;
        for(int i = 0; i < pList.Count; i++) 
        {
            if(inRangeSelected(pList[i].transform.position, character.stats.getAttackRange())) 
            {
                if(attacking != null) 
                {
                    if(firstArgIsCloser(pList[i].transform.position, attacking.transform.position)) 
                    {
                        attacking = pList[i];
                    }
                }
                else
                {
                    attacking = pList[i];
                }
            }
        }
        //if previous target is closest or tied for closest it should be attacked
        if(target != null && attacking != null && firstArgIsCloser(target.transform.position,attacking.transform.position)) 
        {
            return target;
        }
        if(attacking != null) 
        {
            target = attacking;
        }
        return attacking;
    }

    private bool firstArgIsCloser(Vector3 position1, Vector3 position2) {
        return (transform.position - position1).magnitude <= (transform.position - position2).magnitude;
    }

    private void setTarget(GameObject thisCheck, GameObject attacker) 
    {
        if(thisCheck == gameObject) 
        {
            target = attacker;
        }
    }
}
