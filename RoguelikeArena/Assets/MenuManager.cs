using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public static MenuManager Instance { get; private set; }

    void Awake() {
        if (Instance != null) Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private const int mainMenuIndex = 0, gameplayIndex = 1;
    public void GoToMainMenu() {
        SceneManager.LoadScene(mainMenuIndex);
    }
    public void StartGame() {
        SceneManager.LoadScene(gameplayIndex);
    }
    public void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // TODO
    public void OpenAchievements() {

    }
    public void OpenOptions() {

    }
    public void ExitGame() {

    }
}
