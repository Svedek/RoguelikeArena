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


    [SerializeField] Canvas optionsCanvas;
    public void OpenOptions() {
        GetComponent<Canvas>().enabled = false;
        optionsCanvas.enabled = true;
    }
    public void ReturnFromOptions() {
        GetComponent<Canvas>().enabled = true;
        optionsCanvas.enabled = false;
    }


    Canvas canvas;
    private void Awake() {
        canvas = GetComponent<Canvas>();
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state) {
        canvas.enabled = state == GameState.Paused;
        if (state == GameState.Playing) optionsCanvas.enabled = false;
    }
}
