using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Singleton Setup
    public static GameManager Instance;
    
    void Awake() {
        // Singleton Setups
        if (Instance != null) {
            Debug.LogError("Multiple instances of GameManager!");
        }
        Instance = this;
    }
    #endregion

    private void Start() {
        UpdateGameState(GameState.Playing);
    }
    void Update() {
        HandlePause();
        HandleTime();
    }

    #region Game State
    public static event Action<GameState> OnGameStateChanged;

    public GameState State { get; private set; }

    public void UpdateGameState(GameState newState) {
        if (State == newState) return;
        
        State = newState;

        switch (newState) {
            case GameState.Playing:
                break;
            case GameState.Paused:
                break;
            case GameState.Dead:
                break;
            case GameState.Win:
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandlePause() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (State > GameState.Paused) return;
            UpdateGameState(State == GameState.Playing ? GameState.Paused : GameState.Playing);
        }
    }
    #endregion

    #region Time / Scaling
    public float Scaling {
        get {
            return 5 + time * .05f + LevelManager.Instance.Floor * 2f;
        }
    }

    private float time = 0f;

    private void HandleTime() {
        if (State != GameState.Playing) return;
        time += Time.deltaTime;
        UIManager.Instance.SetTime(time);
    }
    #endregion
}

public enum GameState {
    Playing,
    Paused,
    Dead,
    Win
}
