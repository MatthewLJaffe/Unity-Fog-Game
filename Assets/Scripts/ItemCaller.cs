using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCaller : MonoBehaviour
{
    private Effect itemEffect;
    public ItemStats itemStats;

    private void Awake() {
        itemEffect = GetComponent<Effect>();
    }

    public void callEffect() {
        itemEffect.doEffect();
    }
}
