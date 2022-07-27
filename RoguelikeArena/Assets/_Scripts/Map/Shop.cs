using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    [SerializeField] GameObject shopStall, weaponUpgrade;

    void Awake() {
        
    }

    private void Start() {
        // Spawn Things
        SpawnStalls();
        //SpawnWeaponUpgrades();
    }

    #region Spawning
    private const float yOffset = 7.5f;
    private void SpawnStalls() {
        int toSpawn = 4;
        if (toSpawn == 0) return;
        var range = toSpawn * 3f;
        float seperation = range * 2f / toSpawn;

        ShopStall[] stalls = new ShopStall[toSpawn];
        for (int i = 0; i < toSpawn; i++) {
            GameObject stallObject = Instantiate(shopStall, transform);
            stallObject.transform.localPosition = new Vector2(i * seperation - range + seperation/2, -yOffset);

            stalls[i] = stallObject.GetComponent<ShopStall>();
        }
        ItemManager.Instance.PopulateStalls(stalls);
    }

    private void SpawnWeaponUpgrades() {
        int toSpawn = 2;
        if (toSpawn == 0) return;
        var range = toSpawn * 4f;
        float seperation = range * 2f / toSpawn;
        for (int i = 0; i < toSpawn; i++) {
            GameObject stallObject = Instantiate(shopStall, transform);
            stallObject.transform.localPosition = new Vector2(i * seperation - range + seperation / 2, yOffset);
        }
    }
    #endregion
}
