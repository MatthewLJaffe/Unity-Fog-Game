using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;


public class ItemMagnetize : MonoBehaviour {

    private Rigidbody2D rb;

    [SerializeField] private float stickyness;
    private Vector3 stickLocation;
    private bool magnitizing = false;
    private string team;
    private Vector2 lastForce = Vector2.zero;
    private GameObject target;
    private bool attach = false;

    public delegate void useItemData(GameObject itemData, string team);
    public static event useItemData OnItemRecieved = delegate { };

    private void Awake() {
        ItemDropper.OnItemEnable += startMagnitize;
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnDisable() {
        ItemDropper.OnItemEnable -= startMagnitize;
    }

    public void startMagnitize(GameObject piece) {
            target = piece;
            stickLocation = piece.transform.position;
            team = piece.GetComponent<Character>().stats.side;
            rb.velocity = (stickLocation - transform.position) * stickyness;
            magnitizing = true;
    }
    
    private void magnitize() 
    {
        rb.velocity = (stickLocation - transform.position) * stickyness;
        if (attach) {
            Debug.Log("item recieved");
            magnitizing = false;
            OnItemRecieved(gameObject.transform.parent.GetChild(1).gameObject, team);
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate() 
    {
        if(magnitizing) 
        {
            magnitize();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject == target) {
            attach = true;
            Debug.Log("attach");
        }
    }
}
