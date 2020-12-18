using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayActivator : MonoBehaviour
{
    private GameObject thisParent;
    private Character character;


    private void Awake() {
        PlayerController.changeMove += changeMoveOverlay;
        PlayerController.changeAttack += changeAttackOverlay;
        EnemyController.changeMove += changeMoveOverlay;
        EnemyController.changeAttack += changeAttackOverlay;
        thisParent = transform.parent.gameObject;
        IncreaseStat.onStatIncrease += updateRange;
        
    }
    private void OnDisable() {
        PlayerController.changeMove -= changeMoveOverlay;
        PlayerController.changeAttack -= changeAttackOverlay;
        EnemyController.changeMove -= changeMoveOverlay;
        EnemyController.changeAttack -= changeAttackOverlay;
        IncreaseStat.onStatIncrease -= updateRange;


    }
    // Start is called before the first frame update
    private void Start()
    {
        character = transform.GetComponentInParent<Character>();
        updateRange(transform.parent.gameObject);
    }

    private void updateRange(GameObject piece) {
        //sets scale of attack and move overlay acording to move and attack range
        if(piece == transform.parent.gameObject) {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.transform.localScale = new Vector3((1 + 2 * character.stats.getMoveRange())
                * (1 / transform.parent.transform.localScale.x),
                (1 + 2 * character.stats.getMoveRange()) * (1 / transform.parent.transform.localScale.y), 1);
            transform.GetChild(0).gameObject.SetActive(false);

            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.transform.localScale = new Vector3((1 + 2 * character.stats.getAttackRange())
                * (1 / transform.parent.transform.localScale.x), (1 + 2 * character.stats.getAttackRange()) * (1 / transform.parent.transform.localScale.y), 1);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }


    //switches state of move overlay from active to inactive or vice versa
    private void changeMoveOverlay(GameObject parent, bool active) {
        if (parent == thisParent) {
            transform.GetChild(0).gameObject.SetActive(active);
        }
    }

    //switches state of attack overlay from active to inactive or vice versa
    private void changeAttackOverlay(GameObject parent, bool active) {
        if (parent == thisParent) {
            transform.GetChild(1).gameObject.SetActive(active);
        }
    }
}
