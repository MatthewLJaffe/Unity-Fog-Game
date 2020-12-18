using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }
    private bool move = false;
    private bool attack = false;
    private bool buttonPressed = false;

    public delegate void defaultEventArgs();
    public delegate void cancelEventArgs(GameObject go);
    public static event defaultEventArgs OnInventorySelected = delegate { };
    public static event cancelEventArgs OnCancel = delegate { };
    [SerializeField] private GameObject canvas; 

    
    //handles singleton creation
    private void Awake() 
    {
        if(Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void setMove(bool m) 
    {
        GameObject engagedPiece = PieceList.Instance.getEngagedPiece();
        if(engagedPiece != null && !engagedPiece.GetComponent<PlayerController>().myActionEngaged || !m) 
        {
            move = m;
            if (move == true) {
                buttonPressed = true;
            } 
        }      
    }
        
    public void setAttack(bool a) 
    {
        GameObject engagedPiece = PieceList.Instance.getEngagedPiece();
        if (engagedPiece != null && !engagedPiece.GetComponent<PlayerController>().myActionEngaged)
        {
            attack = a;
            if (attack == true) {
                Debug.Log("attack");
                buttonPressed = true;
            } 
        }        
    }

    public void setButtonPressed(bool b) {
        buttonPressed = b;
    }

    public bool getButtonPressed() {
        return buttonPressed;
    }

    public bool getAttack() {
        return attack;
    }

    public bool getMove() {
        return move;
    }

    public void inventorySelected() 
    {
        GameObject engagedPiece = PieceList.Instance.getEngagedPiece();
        if (engagedPiece != null && !engagedPiece.GetComponent<PlayerController>().myActionEngaged) 
        {
            //activate inventory is selected using button use item
            for (int i = 0; i < canvas.transform.childCount; i++) 
            {
                if (canvas.transform.GetChild(i).gameObject.tag.Equals("Inventory") ||
                    canvas.transform.GetChild(i).gameObject.tag.Equals("End")) {
                    canvas.transform.GetChild(i).gameObject.SetActive(true);
                } 
                else {
                    canvas.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            OnInventorySelected();
        }
    }

    public void cancel() {
        OnCancel(PieceList.Instance.getEngagedPiece());
    }

    public void enableButtons() 
    {
        for (int i = 0; i < canvas.transform.childCount; i++) {
            if(canvas.transform.GetChild(i).gameObject.tag.Equals("onPlayer"))
                canvas.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void enableUseItem() 
    {
        //spaghetti code
        for (int i = 0; i < canvas.transform.childCount; i++) 
        {
            if (canvas.transform.GetChild(i).gameObject.name.Equals("Use Item") || 
                canvas.transform.GetChild(i).gameObject.name.Equals("Cancel")  ||
                canvas.transform.GetChild(i).gameObject.name.Equals("Player Stats"))
                canvas.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void disableButtons() 
    {
        for (int i = 0; i < canvas.transform.childCount - 1; i++) 
        {
            if (canvas.transform.GetChild(i).gameObject.tag.Equals("onPlayer") || 
                canvas.transform.GetChild(i).gameObject.tag.Equals("Inventory"))
                canvas.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
