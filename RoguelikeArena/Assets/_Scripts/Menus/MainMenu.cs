using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private const int mainMenuIndex = 0, gameplayIndex = 1;
    

    [SerializeField] GameObject exitButton;
    private void Start() {
        // Remove non-webGL options
        #if UNITY_WEBGL
        exitButton.SetActive(false);
        #endif
    }

    public void StartGame() {
        SceneManager.LoadScene(gameplayIndex);
    }

    // TODO
    [SerializeField] GameObject congratsText;
    public void OpenAchievements() {
        congratsText.SetActive(true);
    }

    [SerializeField] Canvas optionsCanvas;
    public void OpenOptions() {
        GetComponent<Canvas>().enabled = false;
        optionsCanvas.enabled = true;
    }
    public void ReturnFromOptions() {
        GetComponent<Canvas>().enabled = true;
        optionsCanvas.enabled = false;
    }

    public void ExitGame() {
        Application.Quit();
    }

    
}
