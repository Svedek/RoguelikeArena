using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    private const int mainMenuIndex = 0, gameplayIndex = 1;
    public void GoToMainMenu() {
        SceneManager.LoadScene(mainMenuIndex);
    }
    public void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Awake() {
        
    }

    void Update() {
        
    }
}
