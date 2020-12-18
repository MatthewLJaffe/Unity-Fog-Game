using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraMovement : MonoBehaviour
{
    public Camera worldCam;
    private Camera moveCam;
    private PixelPerfectCamera pixCam;
    private Vector3 initialPosition;
    private Vector3 changePosition;
    private float dx;
    private float dy;
    private float moveFactor = .5f;

    private bool movementEnabled = true;
    private Rigidbody2D rb;
    private int PPUFactor = 8;
    [SerializeField] private int maxPPU;
    [SerializeField] private int minPPU;
    [SerializeField] private int arrowMoveSpeed;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        moveCam = GetComponent<Camera>();
        pixCam = GetComponent<PixelPerfectCamera>();
        EnemyCaller.OnEnemyTurn += startPanCamera;
        TurnManager.OnTurnChange += panToSide;


    }
    private void OnDisable() {
        EnemyCaller.OnEnemyTurn -= startPanCamera;
        TurnManager.OnTurnChange -= panToSide;
    }

    void Update() 
    {
        if(movementEnabled) 
        {
            moveWithMouse();
            moveWithKeys();
        }
    }

    private void startPanCamera(Vector3 pos) 
    {
        StartCoroutine(panCamera(pos));
    }

    private void moveWithMouse() 
    {
        if (Input.GetMouseButtonDown(1)) {
            initialPosition = worldCam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(1) && worldCam.ScreenToWorldPoint(Input.mousePosition) != initialPosition) {
            dx = initialPosition.x - worldCam.ScreenToWorldPoint(Input.mousePosition).x;
            dy = initialPosition.y - worldCam.ScreenToWorldPoint(Input.mousePosition).y;
            changePosition = transform.position;
            if (dx < 2 && dy < 2) {
                changePosition.x += dx * moveFactor * (32f / pixCam.assetsPPU);
                changePosition.y += dy * moveFactor * (32f / pixCam.assetsPPU);
            }
            if (ScreenBounds.inScreenBounds(changePosition)) {
                transform.position = changePosition;
            }
            initialPosition = worldCam.ScreenToWorldPoint(Input.mousePosition);
            dx = 0;
            dy = 0;
        }
        
        if(Input.mouseScrollDelta.y != 0) 
        {
            float mouseDy = Input.mouseScrollDelta.y;
            pixCam.assetsPPU += PPUFactor * (int)(mouseDy / Mathf.Abs(mouseDy));
            if (pixCam.assetsPPU >= maxPPU) {
                pixCam.assetsPPU = maxPPU;
            }
            if (pixCam.assetsPPU <= minPPU) {
                pixCam.assetsPPU = minPPU;
            }
        }
        
    }
    private void moveWithKeys() {
        changePosition = transform.position;
        if(Input.GetKey(KeyCode.UpArrow)) {
            changePosition.y += arrowMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            changePosition.y -= arrowMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            changePosition.x += arrowMoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            changePosition.x -= arrowMoveSpeed * Time.deltaTime;
        }
        if (ScreenBounds.inScreenBounds(changePosition)) {
            transform.position = changePosition;
        }
    }

    private IEnumerator panCamera(Vector3 destination) 
    {
        destination.z = transform.position.z;
        movementEnabled = false;
        Vector3 moveDir = destination - transform.position;
        rb.velocity = new Vector3(moveDir.x,moveDir.y,-10);
        yield return new WaitForSeconds(1);
        rb.velocity = Vector3.zero;
        transform.position = destination;
        movementEnabled = true;
    }
    private void panToSide(string side) {
        Vector3 averageLocation = Vector3.zero;
        List<GameObject> pList = PieceList.Instance.pieces;
        for (int i = 0; i <pList.Count; i++) {
            if(pList[i].GetComponent<Character>().stats.side.Equals(side)) {
                averageLocation += pList[i].transform.position;
            }
        }
        averageLocation /= PieceList.Instance.playersOnSide(side);
        StartCoroutine(panCamera(averageLocation));
    }
}
