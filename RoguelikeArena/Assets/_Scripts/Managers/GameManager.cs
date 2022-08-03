using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public float Scaling {
        get {
            return 5 + time * .05f + LevelManager.Instance.Floor * 2f;
        }
    }

    private float time = 0f;

    void Awake() {
        // Singleton Setups
        if (Instance != null) {
            Debug.LogError("Multiple instances of GameManager!");
        }
        Instance = this;
    }

    void Update() {
        time += Time.deltaTime;
        UIManager.Instance.SetTime(time,Scaling);
    }
}
