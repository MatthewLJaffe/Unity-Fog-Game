using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Conversions : MonoBehaviour
{

    public static Conversions Instance { get; private set; }

    private Grid fog;
    private Camera mainCam; 


    private void Awake() {
        fog = GameObject.FindGameObjectWithTag("MainGrid").GetComponent<Grid>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else {
            Destroy(gameObject);
        }
    }

    public Vector3Int getGridSpace(Vector3 coordinates) {
        return fog.WorldToCell(coordinates);
    }

    public Vector3 mousePosition() {
        return mainCam.ScreenToWorldPoint(Input.mousePosition);
    }

}
