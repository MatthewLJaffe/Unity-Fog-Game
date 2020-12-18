using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class FogAway : MonoBehaviour {
    public delegate void CheckRevealed(FogAway away);
    public static event CheckRevealed OnFogRevealed = delegate { };
    private static Tilemap fog;
    private static bool fogNotSet = true;

    public void Awake() {
        if(fogNotSet) {
            fog = GameObject.FindGameObjectWithTag("Fog").GetComponent<Tilemap>();
            fogNotSet = false;
        }        
    }
    public void Start() {
        if(GetComponent<Character>() != null) {
            uncover(2);
        }
    }

    public void uncover(int uncoverRange) {
        Vector3Int tileLocation;
        for (int x = -uncoverRange; x <= uncoverRange; x++) {
            for (int y = -uncoverRange; y <= uncoverRange; y++) {
                tileLocation = Conversions.Instance.getGridSpace((new Vector3(transform.position.x + x,
                    transform.position.y + y, transform.position.z)));
                fog.SetTile(tileLocation, null);
            }
        }
        OnFogRevealed(this);
    }

    public static bool isFog(Vector3Int pos) {
        return fog.GetTile(pos) != null;
    }

}
