using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitioner : MonoBehaviour
{
    public Animator transition;
    private static int previousIndex;
    private void Awake() {
        WinTrigger.OnLose += startSceneTransition;
        Attack.OnLose += startSceneTransition;
    }

    public void startSceneTransition() {
        if(this != null) { //incase this gets called by stopping run
            StartCoroutine(sceneTransition(SceneManager.GetActiveScene().buildIndex + 1));
        }  
    }

    public void switchBetewenRules() {
        if(SceneManager.GetActiveScene().name.Equals("Rules")) {
            SceneManager.LoadScene(previousIndex);
        }
        else {
            previousIndex = SceneManager.GetActiveScene().buildIndex;
            Debug.Log(previousIndex);
            SceneManager.LoadScene("Rules");
        }
    }

    private IEnumerator sceneTransition(int sceneIndex) {
        previousIndex = SceneManager.GetActiveScene().buildIndex;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }

    public void Quit() {
        Application.Quit();
    }
}