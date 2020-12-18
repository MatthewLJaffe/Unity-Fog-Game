using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinText : MonoBehaviour
{
    [SerializeField] private WinData winData;
    private TextMeshProUGUI endText;

    private void Awake() {
        endText = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        string loser;
        string winner;

        if(winData.loser.Equals("Red")) {
            loser = "Red";
            winner = "Blue";
        } 
        else {
            loser = "Blue";
            winner = "Red";
        }
        endText.text = winner + " player has defeated " + loser + " player" + System.Environment.NewLine
         + "Thanks for playing!";
    }
}
