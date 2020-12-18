using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    public PartyMemberData stats;
    private Action<GameObject> actionAvailable;
    private Rigidbody2D rb;
    public HealthManager health;
    private Animator anim;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        actionAvailable = GetComponent<Action<GameObject>>();
        rb = GetComponent<Rigidbody2D>();
        stats.resetStats();
        health = transform.GetChild(1).transform.GetChild(0).GetComponent<HealthManager>(); //kill me
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start() {
        health.setMaxHealth(stats.hp);
    }

    public IEnumerator moveTo(Vector3 movePosition) {
        if(anim != null) {
            anim.SetBool("Walking", true);
        }
        Vector3Int tilePosition = Conversions.Instance.getGridSpace
            (new Vector3(movePosition.x + 1, movePosition.y + 1, movePosition.z)); //dunno why
        Vector2 movementDirection = tilePosition - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, movementDirection,
            movementDirection.magnitude, LayerMask.GetMask("Impassable"));
        if (hit) //see if something in front of tile assigned to
        {
            Vector3 moveArea = hit.point - movementDirection.normalized / 1.5f;
            tilePosition = Conversions.Instance.getGridSpace(new Vector3(moveArea.x + 1, moveArea.y + 1, 0)); //still dunno why
            movementDirection = tilePosition - transform.position;
        }
        if(movementDirection.x != 0) {
            spriteRenderer.flipX = movementDirection.x < 0;
        }
        rb.velocity = movementDirection;
        yield return new WaitForSeconds(1);
        rb.velocity = Vector2.zero;
        transform.position = tilePosition;
        if(anim != null) {
            anim.SetBool("Walking", false);
        }
        if (GetComponent<FogAway>() != null) 
        {
            GetComponent<FogAway>().uncover(stats.getUncoverRange());
        }
    }

    public void action(GameObject reciever) {
        actionAvailable.doAction(reciever);
    }
          
}
