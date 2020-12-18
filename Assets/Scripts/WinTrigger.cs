using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class WinTrigger : MonoBehaviour
{
    string side;
    [SerializeField] private WinData winData;
    public delegate void loseArgs();
    public static event loseArgs OnLose = delegate { };
    private void Awake() {
        side = GetComponent<Character>().stats.side;
    }

    private void OnDisable() 
    {
        winData.loser = side;
        OnLose();
        Debug.Log(side);
    }

}