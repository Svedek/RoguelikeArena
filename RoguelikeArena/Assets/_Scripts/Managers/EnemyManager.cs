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

    public void SpawnEnemies(RoomBase room, Vector2 enterDir) {
        currentRoom = room;
        Vector2 roomCenter = room.transform.position;

        float spawnVal = GameManager.Instance.Scaling;

        List<GameObject> toSpawn = new List<GameObject>();

        int spawnGen;
        switch (spawnVal) {
            case float a when(a < 20f): // Tier 1
                spawnGen = 0;
                break;
            case float a when (a < 25f): // Tier 1 and 2
                spawnGen = 1;
                break;
            case float a when (a < 50f): // Tier 2
                spawnGen = 2;
                break;
            case float a when (a < 55f): // Tier 2 and 3
                spawnGen = 3;
                break;
            default: // Tier 3
                spawnGen = 4;
                break;
        }
        
        while (spawnVal > (1+(spawnGen/2))*2) {
            float currentSpawnVal = spawnVal * Random.Range(.4f, 1f);
            int spawnType = SpawnType(spawnGen, currentSpawnVal);
            if (spawnType == -1) {
                Debug.LogWarning("While calculation is wrong if this goes off");
                break;
            }

            var cost = enemyCostList[spawnType];
            int count = (int) (currentSpawnVal / cost);
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

    private readonly EnemyID[][] enemySpawnGen = new EnemyID[][] {
        new EnemyID[]{EnemyID.Swarmer1, EnemyID.Runner1, EnemyID.Shooter1},
        new EnemyID[]{EnemyID.Swarmer1, EnemyID.Runner1, EnemyID.Shooter1, EnemyID.Swarmer2, EnemyID.Runner2, EnemyID.Shooter2},
        new EnemyID[]{EnemyID.Swarmer2, EnemyID.Runner2, EnemyID.Shooter2},
        new EnemyID[]{EnemyID.Swarmer2, EnemyID.Runner2, EnemyID.Shooter2, EnemyID.Runner3, EnemyID.Shooter3, EnemyID.Sniper},
        new EnemyID[]{EnemyID.Runner3, EnemyID.Shooter3, EnemyID.Sniper},
    };

    private int SpawnType(int spawnGen, float spawnVal) {
        EnemyID[] spawnList = enemySpawnGen[spawnGen];

        int i;
        for (i = 0; i < spawnList.Length; i++) {
            if (spawnVal < enemyCostList[(int)spawnList[i]]) break;
        }

        if (i == 0) return -1;
        else return (int)spawnList[Random.Range(0,i)];
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
    private static readonly float[] enemyCostList = CostList();
    private static float[] CostList() {
        var costList = new float[System.Enum.GetValues(typeof(EnemyID)).Length];

        // 0 - 25
        costList[(int)EnemyID.Swarmer1] = 0.5f;
        costList[(int)EnemyID.Runner1] = 1f;
        costList[(int)EnemyID.Shooter1] = 3f;
        // 20 - 55
        costList[(int)EnemyID.Swarmer2] = 1f;
        costList[(int)EnemyID.Runner2] = 2f;
        costList[(int)EnemyID.Shooter2] = 5f;
        // 50+
        costList[(int)EnemyID.Runner3] = 3f;
        costList[(int)EnemyID.Shooter3] = 7f;
        costList[(int)EnemyID.Sniper] = 10f;

        return costList;
    }
    private enum EnemyID {
        Swarmer1,
        Runner1,
        Shooter1,
        Swarmer2,
        Runner2,
        Shooter2,
        Runner3,
        Shooter3,
        Sniper,
    }
    #endregion
}
