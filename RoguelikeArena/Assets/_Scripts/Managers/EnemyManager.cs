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

        float spawnVal = Random.Range(scaling * 0.75f, scaling*1.25f);

        List<GameObject> toSpawn = new List<GameObject>();

        while (spawnVal > 5f) {
            float currentSpawnVal = Random.Range(spawnVal/4, spawnVal);
            int spawnType = SpawnType(currentSpawnVal);

            var cost = enemyCostList[spawnType];
            int count = (int) currentSpawnVal / cost;
            spawnVal -= count * cost;
            for (int i = 0; i < count; i++) {
                toSpawn.Add(enemyPrefabs[spawnType]);
            }

        }

        // Find Spawn Area
        Vector2[] spawnBounds = FindArea(enterDir);


        // Spawn Enemies
        enemylist = new List<Enemy>();
        for (int i = 0; i < toSpawn.Count; i++) {
            Vector2 pos = new Vector2(Random.Range(spawnBounds[0].x, spawnBounds[1].x), Random.Range(spawnBounds[0].y, spawnBounds[1].y));
            GameObject current = Instantiate(toSpawn[i], transform);
            current.transform.position = pos;
            enemylist.Add(current.GetComponent<Enemy>());
        }
    }


    private int SpawnType(float spawnVal) {
        switch(spawnVal) {
            case float a when (a < enemyCostList[1]):
                return 0;
            case float a when (a < enemyCostList[2]):
                return Random.Range(0,2);
        }

        return Random.Range(0, enemyCostList.Length); ;
     }


    private readonly float roomSize = 18f, entryPadding = 9f;
    private Vector2[] FindArea(Vector2 dir) {
        Vector2 roomPos = currentRoom.transform.position;

        Vector2 low = roomPos + Vector2.one * -roomSize;
        Vector2 high = roomPos + Vector2.one * roomSize;
        Vector2[] ret = {
            low,
            high
        };

        switch (dir.x, dir.y) {
            case (1, 0): // Right
                ret[1].x -= entryPadding;
                return ret;
            case (0, 1): // Up
                ret[1].y -= entryPadding;
                return ret;
            case (-1, 0): // Left
                ret[0].x += entryPadding;
                return ret;
            case (0, -1): // Down
                ret[0].y += entryPadding;
                return ret;
        }
        Debug.LogError("Incorrect dir in ModifyPosition");
        return ret;
    }

    public void EnemyDied(Enemy enemy) {
        if (!enemylist.Remove(enemy)) Debug.LogError("??? Enemy Died");
        if (enemylist.Count == 0) RoomCleared();
    }
    private void RoomCleared() {
        currentRoom.RoomCleared();
    }
    #endregion

    #region EnemyIDing and cost
    private static readonly int[] enemyCostList = CostList();
    private static int[] CostList() {
        var costList = new int[System.Enum.GetValues(typeof(EnemyID)).Length];

        costList[(int)EnemyID.Swarmer] = 1;
        costList[(int)EnemyID.Runner] = 2;
        costList[(int)EnemyID.Shooter] = 5;

        return costList;
    }
    private enum EnemyID {
        Swarmer,
        Runner,
        Shooter
    }
    #endregion
}
