using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceList : MonoBehaviour
{
    public static PieceList Instance  { get; private set;}
    public List<GameObject> pieces;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else {
            Destroy(gameObject);
        }

        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < p.Length; i++) {
            pieces.Add(p[i]);
        }
    }

    public GameObject getEngagedPiece() {
        for (int i = 0; i < pieces.Count; i++) {
            if (pieces[i].GetComponent<PlayerController>().myActionActive) {
                return pieces[i];
            }
        }
        return null;
    }

    public int playersOnSide(string side) {
        int sidePlayers = 0;
        for(int i = 0; i < pieces.Count; i++) {
            if(pieces[i].GetComponent<Character>().stats.side.Equals(side)) {
                sidePlayers += 1;
            } 
        }
        return sidePlayers;
    }
}
