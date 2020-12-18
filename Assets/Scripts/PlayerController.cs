using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class PlayerController : Controller
{
    public bool healer;
    public bool myActionActive;
    public bool myActionEngaged = false;
    [SerializeField]
    private TextMeshProUGUI statDisplay;
    public bool actionUsed = false;
    private Color selectedColor;
    private Color initialColor;
    private Vector3 actionPosition;
    private Conversions conversions;
    private PartyMemberData stats;

    public delegate void ActionOn(GameObject go, bool set);
    public delegate void SelectArgs();

    public static event SelectArgs OnSelect = delegate { };
    public static event ActionOn changeMove = delegate { };
    public static event ActionOn changeAttack = delegate { };


    private void Awake() {
        // may want to try delegate events instead of caching
        character = GetComponent<Character>();
        
        UIManager.OnCancel += tryDisableActionOptions;
        IncreaseStat.onStatIncrease += updateStats;
        stats = character.stats;
    }
    private void OnDisable() {
        UIManager.OnCancel -= tryDisableActionOptions;
        IncreaseStat.onStatIncrease -= updateStats;

    }

    void Start()
    {
        conversions = Conversions.Instance;
        initialColor = GetComponent<SpriteRenderer>().color;
        selectedColor = new Color(initialColor.r+.25f, initialColor.g + .25f, initialColor.b - .25f);
        UIManager.Instance.setMove(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) 
        {
            if (canSelect()) { // clicking on character
                showActionOptions();
            }
            else if(canSelect()) {

            }
            else if(myActionActive)
            {
                if(UIManager.Instance.getMove()) {
                    initializeMove();
                } 
                else if(UIManager.Instance.getAttack()) { //attack
                    initializeAction();
                }
            }
            if (!gameObject.GetComponentInParent<TurnManager>().isTurn()) {
                actionUsed = false;
            }
        }
    }

    private bool canSelect() 
    {
        return inRangeSelected(conversions.mousePosition(), 0) && PieceList.Instance.getEngagedPiece() == null && 
            gameObject.GetComponentInParent<TurnManager>().isTurn();
    }

    
    private void showActionOptions() 
    {
        GetComponent<SpriteRenderer>().color = selectedColor;
        myActionActive = true;
        updateStats(gameObject);
        if (!actionUsed) {
            UIManager.Instance.enableButtons();
            OnSelect();
        } 
        else {
            UIManager.Instance.enableUseItem();
        }
    }

    private void updateStats(GameObject piece) {
        string action;
        if(healer) {
            action = "Healing: ";
        } 
        else {
            action = "Attack: ";
        }
        if(piece == gameObject) 
        {
            statDisplay.SetText(stats.pieceName
                + System.Environment.NewLine + "HP: " + GetComponent<Character>().health.currentHealth
                + System.Environment.NewLine + action + stats.getDamage()
                + System.Environment.NewLine + "Attack Range: " + stats.getAttackRange()
                + System.Environment.NewLine + "Move Range: " + stats.getMoveRange()
                + System.Environment.NewLine + "Uncover Range: " + stats.getUncoverRange());
        }
    }

    private void tryDisableActionOptions(GameObject go) {
        if(go == gameObject) {
            disableActionOptions();
        }
    }
    private void disableActionOptions() 
    {
        if (UIManager.Instance.getMove()) 
        {
            changeMove(gameObject,false);
            UIManager.Instance.setMove(false);
        }
        if (UIManager.Instance.getAttack())
        {
            changeAttack(gameObject,false);
            UIManager.Instance.setAttack(false);
        }
        GetComponent<SpriteRenderer>().color = initialColor;
        UIManager.Instance.disableButtons();
        myActionActive = false;
        myActionEngaged = false;
    }

    private void initializeMove() 
    {
        if (!mustAttack())
        {
            myActionEngaged = true;
            if(UIManager.Instance.getButtonPressed()) 
            {
                changeMove(gameObject,true);
                UIManager.Instance.setButtonPressed(false);
            }
            else if (inRangeSelected(conversions.mousePosition(),stats.getMoveRange()) && freeSpace())
            {
                actionPosition = conversions.mousePosition();
                StartCoroutine(character.moveTo(actionPosition));
                actionUsed = true;
                disableActionOptions();
            } 
        }
        else
        {
            disableActionOptions();
        }
    }

    private void initializeAction() 
    {
        myActionEngaged = true;
        if (UIManager.Instance.getButtonPressed()) 
        {
            UIManager.Instance.setButtonPressed(false);
            changeAttack(gameObject,true);
        }
        else if (inRangeSelected(conversions.mousePosition(), stats.getAttackRange())) 
        {
            GameObject reciever;
            if(healer) {
                reciever = getHealing();
            } 
            else {
                Debug.Log("trying");
                reciever = getAttacking();
            }
            if (reciever != null) 
            {
                character.action(reciever);
                actionUsed = true;
                disableActionOptions();
            }
        } 
    }
    new private bool mustAttack() 
    {
        for(float x = transform.position.x - 1; x <= transform.position.x + 1; x++) 
        {
            for(float y = transform.position.y - 1; y <= transform.position.y + 1; y++) 
            {
                GameObject potentialEntity = SpaceCheck.Instance.getOccupied(new Vector3(x, y, 0));
                if(potentialEntity != null && potentialEntity.GetComponent<Character>() != null && 
                    !potentialEntity.GetComponent<Character>().stats.side.Equals(stats.side)) {
                    return true;
                }
            }
        }
        return false;
    }

    private bool freeSpace() 
    {
        Vector3 movePosition = conversions.mousePosition();
        Vector3Int tilePosition = Conversions.Instance.getGridSpace
            (new Vector3(movePosition.x + 1, movePosition.y + 1, movePosition.z)); //dunno why
        Vector2 movementDirection = tilePosition - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, movementDirection, movementDirection.magnitude, LayerMask.GetMask("Impassable"));
        if(hit)
        {
            if(!FogAway.isFog(conversions.getGridSpace(hit.point))) {
                return false;
            }
        }
        return true;
    }

    private GameObject getAttacking() 
    {
        GameObject sel = EntitySelector.selected;
        if (sel != null && !FogAway.isFog(conversions.getGridSpace(sel.transform.position)) && 
            !sel.GetComponent<Character>().stats.side.Equals(character.stats.side)) 
        {
            Debug.Log(sel.name);
            return sel; 
        }
    return null;
    }

    private GameObject getHealing() 
    {
        if (EntitySelector.selected != null && EntitySelector.selected.GetComponent<Character>().stats.side.Equals(character.stats.side)) 
        {
            return EntitySelector.selected;
        }
        return null;
    }
}