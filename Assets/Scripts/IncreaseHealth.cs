using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealth : MonoBehaviour, Effect
{
    public void doEffect() {
        HealthManager healthManager = PieceList.Instance.getEngagedPiece().GetComponent<Character>().health;
        healthManager.setMaxHealth((int)healthManager.slider.maxValue + 2);
    }
}
