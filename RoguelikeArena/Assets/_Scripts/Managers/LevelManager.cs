using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    #region Singleton Setup
    public static LevelManager Instance;

    void Awake() {
        // Singleton Setup
        if (Instance != null) {
            Debug.LogError("Multiple instances of LevelManager!");
        }
        Instance = this;

        // Temperary
        CreateLevel();
    }
    #endregion

    #region Level Management
    private Level level;
    [SerializeField] GameObject levelPrefab;

    public int floor { get; private set; } = 0;
    public float scaling {
        get {
            // get game time
            var gameTime = 100f;

            return (gameTime / 10) + (floor * 3);
        }
    }
    public void CreateLevel() {
        ++floor;

        ClearLevel();
        level = Instantiate(levelPrefab).GetComponent<Level>();
        level.Initialize(floor, scaling);
    }

    private void ClearLevel() {
        if (level != null) {
            Destroy(level.gameObject);
        }
    }
    #endregion
}
