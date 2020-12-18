using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ActionText : MonoBehaviour
{
    TextMeshProUGUI actionText;
    // Start is called before the first frame update
    void Awake()
    {
        actionText = GetComponent<TextMeshProUGUI>();
        PlayerController.OnSelect += setActionText;
    }

    private void setActionText() {
        if(PieceList.Instance.getEngagedPiece().GetComponent<PlayerController>().healer) 
        {
            actionText.SetText("Heal");
        } 
        else 
        {
            actionText.SetText("Attack");
        }
    }

}
