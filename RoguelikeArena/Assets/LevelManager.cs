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
        CreateLevel(3,10f);

    }
    #endregion

    #region Level Management
    private Level level;
    [SerializeField] GameObject levelPrefab;

    public void CreateLevel(int floor, float scaling) {
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
