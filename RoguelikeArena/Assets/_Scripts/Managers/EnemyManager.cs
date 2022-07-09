using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager Instance;

    private RoomBase currentRoom;
    private List<Enemy> enemylist;

    void Awake() {
        // Singleton Setup
        if (Instance != null) {
            Debug.LogError("Multiple instances of EnemyManager!");
        }
        Instance = this;
    }

    #region Spawning in room
    [Header("Enemies")]
    [SerializeField] GameObject[] enemyPrefabs;
    struct EnemyData {
        public Vector2 position;
        public GameObject enemyObject;
        public EnemyData(GameObject enemyObject, Vector2 position) {
            this.position = position;
            this.enemyObject = enemyObject;
        }
    }

    public void SpawnEnemies(float scaling, RoomBase room, Vector2 enterDir) {
        currentRoom = room;
        Vector2 roomCenter = room.transform.position;

        float spawnVal = Random.Range(Mathf.Sqrt(scaling), scaling);
        List<EnemyData> toSpawn = new List<EnemyData>();
        switch (spawnVal) {
            case float a when (a <= 2):
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, -10), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, 0), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, 10), enterDir)));
                print("spawn 1-1");
                break;
            case float a when (a <= 4):
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, -10), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-5, -10), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, 0), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-5, 10), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, 10), enterDir)));
                print("spawn 1-2");
                break;
            case float a when (a <= 6):
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, -15), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, -10), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-5, -10), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, 0), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-5, 10), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, 10), enterDir)));
                toSpawn.Add(new EnemyData(enemyPrefabs[0], FindPosition(new Vector2(-10, 15), enterDir)));
                print("spawn 1-3");
                break;
            case float a when (a <= 8):
                print("spawn 2-1");
                break;
            case float a when (a <= 10):
                print("spawn 2-2");
                break;
            case float a when (a <= 12):
                print("spawn 2-3");
                break;
            case float a when (a <= 14):
                print("spawn 3-1");
                break;
            case float a when (a <= 16):
                print("spawn 3-2");
                break;
        }
        enemylist = new List<Enemy>();
        for (int i = 0; i < toSpawn.Count; i++) {
            GameObject current = Instantiate(toSpawn[i].enemyObject, transform);
            current.transform.position = toSpawn[i].position;
            enemylist.Add(current.GetComponent<Enemy>());
        }
    }

    private Vector2 FindPosition(Vector2 position, Vector2 dir) {
        Vector2 roomPos = currentRoom.transform.position;
        switch(dir.x,dir.y) {
            case (1, 0): // Right
                return roomPos + position;
            case (0, 1): // Up
                return roomPos + new Vector2(-position.y, position.x);
            case (-1, 0): // Left
                return roomPos + new Vector2(-position.x, -position.y);
            case (0, -1): // Down
                return roomPos + new Vector2(position.y, -position.x);
        }
        Debug.LogError("Incorrect dir in ModifyPosition");
        return new Vector2(-1, -1);
    }

    public void EnemyDied(Enemy enemy) {
        if (!enemylist.Remove(enemy)) Debug.LogError("??? Enemy Died");
        if (enemylist.Count == 0) RoomCleared();
    }
    private void RoomCleared() {
        currentRoom.RoomCleared();
    }
    #endregion
}
