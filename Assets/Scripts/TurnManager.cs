using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private int myTurnNumber;
    [SerializeField]
    private GameObject endTurn;
    private static int numberOfTurns = 3;
    public static int CURRENT_TURN = 1;
    public static string PLAYER_1_NAME = "Blue";
    public static string PLAYER_2_NAME = "Red";
    public delegate void TurnAdvancedArgs(string team);
    public static event TurnAdvancedArgs OnTurnChange = delegate { };

    public void advanceTurn() 
    {
        CURRENT_TURN++;
        if (CURRENT_TURN > numberOfTurns) 
        {
            CURRENT_TURN = 1;
            OnTurnChange(PLAYER_1_NAME);
        }
        if(CURRENT_TURN == 2) 
        {
            OnTurnChange(PLAYER_2_NAME);
        }
        if (CURRENT_TURN == numberOfTurns) 
        {
            endTurn.SetActive(false);
            Debug.Log("turn 3");
        } 
        else 
        {
            endTurn.SetActive(true);
        }
    }

    public bool isTurn() {
        return myTurnNumber == CURRENT_TURN;
    }
}
