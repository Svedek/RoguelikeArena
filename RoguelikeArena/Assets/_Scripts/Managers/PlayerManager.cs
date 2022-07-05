using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerManager { 

    // Player Stats - manages how player plays

    // Manager Stats - manages systems
    private static float currentExp = 0f, requiredExp = 11f;
    private static int level = 1, gold = 0;

    #region Initialization
    public static void Initialize() {
        SetLevelExpRequirement();
    }

    public static void Reset() {
        currentExp = gold = 0;
        level = 1;
        SetLevelExpRequirement();

    }
    #endregion

    #region Save / Load
    public static void Save() {

    }
    public static void Load() {

    }
    #endregion

    #region Experience and Levels
    public static void GainExperience(float experience) {
        Debug.Log("cXp: "+currentExp + " XpG: "+ experience+" RXp: "+requiredExp);
        if ((currentExp += experience) >= requiredExp) {
            // Handle Experience
            currentExp -= requiredExp;
            LevelUp();
            GainExperience(0f); // Allows for multiple level ups

        }
        // Update UI
        ReferanceManager.uiManager.UpdateExperience(currentExp, requiredExp, level);
    }
    private static void LevelUp() {
        // Handle Experience
        ++level;
        SetLevelExpRequirement();

        // Allow to pick upgrade (small UI popup with prompts, not pausing game) + Heal

        // UI Effect
    }
    private static void SetLevelExpRequirement() {
        // (1,11) (2,16.2) (3, 22.5) (4,29.6) (5,37.4)
        requiredExp = Mathf.Pow(level, 1.7f) + (level * 3) + (7);
    }
    #endregion

    #region Gold
    public static void GainGold(int addGold) {
        gold += addGold;

        // Update UI
    }

    public static bool TryPurchace(int cost) {
        if (gold >= cost) {
            gold -= cost;
            return true;
        }
        return false;
    }
    #endregion
}
