using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    [SerializeField] GameObject shopStall;

    private ShopStall[] itemStalls;
    private ShopStall[] weaponStalls;

    private void Start() {
        // Spawn Things
        SpawnStalls();
        SpawnWeaponUpgrades();
    }

    #region Spawning

    private const float yOffset = 7.5f;
    private void SpawnStalls() {
        int toSpawn = 4;
        if (toSpawn == 0) return;
        var range = toSpawn * 3f;
        float seperation = range * 2f / toSpawn;

        itemStalls = new ShopStall[toSpawn];
        for (int i = 0; i < toSpawn; i++) {
            GameObject stallObject = Instantiate(shopStall, transform);
            stallObject.transform.localPosition = new Vector2(i * seperation - range + seperation/2, -yOffset);

            itemStalls[i] = stallObject.GetComponent<ShopStall>();
        }
        ItemManager.Instance.PopulateItems(itemStalls);
    }

    private void SpawnWeaponUpgrades() {
        GameObject[] weaponsToSpawn = ItemManager.Instance.WeaponsToSpawn();
        if (weaponsToSpawn == null) return;
        int toSpawn = weaponsToSpawn.Length;
        if (toSpawn == 0) return;
        var range = toSpawn * 4f;
        float seperation = range * 2f / toSpawn;

        weaponStalls = new ShopStall[toSpawn];
        for (int i = 0; i < toSpawn; i++) {
            GameObject stallObject = Instantiate(shopStall, transform);
            stallObject.transform.localPosition = new Vector2(i * seperation - range + seperation / 2, yOffset);

            weaponStalls[i] = stallObject.GetComponent<ShopStall>();
            weaponStalls[i].Initialize(weaponsToSpawn[i]);
        }
    }
    #endregion
}
