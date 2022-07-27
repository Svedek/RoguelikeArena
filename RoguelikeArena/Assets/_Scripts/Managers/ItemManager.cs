using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public static ItemManager Instance;

    void Awake() {
        // Singleton Setup
        if (Instance != null) {
            Debug.LogError("Multiple instances of ItemManager!");
        }
        Instance = this;
    }

    #region Populate
    private const int minimumRelics = 0;
    public void PopulateStalls(ShopStall[] stalls) {
        var toPopulate = stalls.Length;


        for (int i = 0; i < toPopulate; i++) { // TODO (right now it only populates w potions)
            int id = Random.Range(0, potions.Length);
            stalls[i].Initialize(potions[id]);
        }
    }
    #endregion

    #region Potions 
    [Header("Potions")]
    [SerializeField] GameObject[] potions;
    private enum PotionID {
        smolPot,
        medPot,
        largePot,
    }
    #endregion

    #region Relics
    [Header("Relics")]
    [SerializeField] GameObject[] relics;
    private static List<Relic.RelicID> relicsAvailable = ((Relic.RelicID[])System.Enum.GetValues(typeof(Relic.RelicID))).ToList();
    #endregion
}
