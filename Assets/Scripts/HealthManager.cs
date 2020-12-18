using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Slider slider;
    public int currentHealth;
    
    public void setMaxHealth(int health) 
    {
        slider.maxValue = health;
        slider.value = health;
        currentHealth = health;
    }

    public void changeHealth(int change) 
    {
        slider.value += change;
        currentHealth += change;
    }
    
    
}
