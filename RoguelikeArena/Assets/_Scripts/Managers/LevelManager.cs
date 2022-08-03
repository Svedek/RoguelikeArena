using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {


    public int Floor { get { return floor; } }

    #region Singleton Setup
    public static LevelManager Instance;

    void Awake() {
        // Singleton Setup
        if (Instance != null) {
            Debug.LogError("Multiple instances of LevelManager!");
        }
        Instance = this;
    }
    #endregion

    private void Start() {
        // TODO, MAYBE MOVE??
        CreateLevel();
    }

    #region Level Management
    private Level level;
    [SerializeField] GameObject levelPrefab;

    private int floor = 0;
    public void CreateLevel() {
        ++floor;

        ClearLevel();
        level = Instantiate(levelPrefab).GetComponent<Level>();
        level.Initialize(GameManager.Instance.Scaling);
    }

    private void ClearLevel() {
        if (level != null) {
            Destroy(level.gameObject);
        }
    }
    #endregion
}
