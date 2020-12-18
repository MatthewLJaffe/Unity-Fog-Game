using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ChestDropper : ItemDropper {
    private Tilemap fog;
    public FogAway clearer;
    private bool notSpawned = true;
    private Animator anim;

    private void Awake() {
        fog = GameObject.FindGameObjectWithTag("Fog").GetComponent<Tilemap>();
        FogAway.OnFogRevealed += ChestFound;
        anim = GetComponent<Animator>();
    }
    private void OnDisable() {
        FogAway.OnFogRevealed -= ChestFound;
    }

    private void ChestFound(FogAway away) {
        if (fog.GetTile(Conversions.Instance.getGridSpace(transform.position)) == null && notSpawned &&
            (transform.position - away.transform.position).magnitude <= 1.5) 
        {
            clearer = away;
            StartCoroutine(EnableItem());
        }
    }

    //waits for fog to clear and chest open animation
    public IEnumerator EnableItem() {
        yield return new WaitForSeconds(.5f);
        anim.SetTrigger("Open");
        yield return new WaitForSeconds(1);
        InstantiateItem(transform, clearer.gameObject);
        notSpawned = false;
    }
}