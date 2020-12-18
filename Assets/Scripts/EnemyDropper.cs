using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropper : ItemDropper
{
    [SerializeField] private int drops;
    [SerializeField] private int dropRate;
    public GameObject destroyer;

    private void OnDisable() 
    {
        if(destroyer != null && dropRate > Random.Range((int)0 ,100))
        {
            for (int i = 0; i < drops; i++) {
                InstantiateItem(transform, destroyer);
                Debug.Log("DROP");
            }
        }
    }
}
