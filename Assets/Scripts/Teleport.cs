using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, Effect
{
    [SerializeField] private GameObject overlay;
    private bool teleport = false;

    private void Awake() {
        overlay = GameObject.FindGameObjectWithTag("Selector").transform.GetChild(0).gameObject;
    }

    public void doEffect()
    {
        overlay.SetActive(true);
        teleport = true;
    }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0) && teleport) 
        {
            Vector3 position = Conversions.Instance.getGridSpace(Conversions.Instance.mousePosition()) + Vector3.up + Vector3.right;
            if(canTeleport(position))
            {
                GameObject active = PieceList.Instance.getEngagedPiece();
                active.transform.SetPositionAndRotation(position, gameObject.transform.rotation);
                active.GetComponent<FogAway>().uncover(1);
                overlay.SetActive(false);
                teleport = false;
                UIManager.Instance.cancel();
            }
        }
    }

    private bool canTeleport(Vector3 position) 
    {
        return !Physics2D.Raycast( position + Vector3.down, Vector3.up, 1, LayerMask.GetMask("Impassable"));
    }
}
