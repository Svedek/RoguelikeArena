using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    // THIS IS GOING TO KEEP TRACK OF GAME TIME

    public static GameplayManager Instance;

    void Awake() {
        // Singleton Setup
        if (Instance != null) {
            Debug.LogError("Multiple instances of GameplayManager!");
        }
        Instance = this;
    }

    void Update() {
        
    }
}
