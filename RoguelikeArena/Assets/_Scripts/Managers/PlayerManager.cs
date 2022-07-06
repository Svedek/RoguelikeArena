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
    private static int upgradePoints = 0;
    private static void LevelUp() {
        // Handle Experience
        ++level;
        SetLevelExpRequirement();

        // Allow to pick upgrade (small UI popup with prompts, not pausing game) + Heal
        ++upgradePoints;
        HandleLevelUpgrades();

        // UI Effect
    }
    private static void SetLevelExpRequirement() {
        // (1,11) (2,16.2) (3, 22.5) (4,29.6) (5,37.4)
        requiredExp = Mathf.Pow(level, 1.7f) + (level * 3) + (7);
    }
    private static bool upgradesActive = false;
    private static void HandleLevelUpgrades() {
        int[] ids = new int[3];
        Color[] colors = new Color[3];
        string[] text = new string[3];
        if (!upgradesActive) {
            for (int i = 0; i < 3; i++) {
                float val = Random.Range(0f, 100f);
                ids[i] = ValToID(val,i);
                colors[i] = upgradeColors[ids[i]];
                text[i] = upgradeText[ids[i]];
            }


            ReferanceManager.uiManager.SetUpgradeOptions(colors,text);

            upgradesActive = true;
        }
    }

    private readonly static Color[] upgradeColors = {
        new Color(255,0,0), // Damage +
        new Color(0,175,0), // Attack speed + 
        new Color(0,175,255), // Move speed +
        new Color(75,75,75), // Defence +
        new Color(150,75,75), // Max health 
        new Color(0,0,225), // Exp +
        new Color(175,175,0) // Gold +
        };
    private readonly static string[] upgradeText = {
        "+Dmg", // Damage +
        "+Atk\nSpd", // Attack speed + 
        "+Mov\nSpd", // Move speed +
        "+Def", // Defence +
        "+Max\nHp", // Max health 
        "+Exp", // Exp +
        "+$$$" // Gold +
        };

    // val defines the upgrade and id defines the pool val chooses from
    private static int ValToID(float val, int id) {
        switch (id) {
            case 0: // 1st upgrade - damage oriented
                switch (val) {
                    case float a when (a <= 50f): // Damage +
                        return 0;
                    case float a when (a <= 100f): // Attack speed +
                        return 1;
                }
                break;
            case 1: // 2nd upgrade - survivability oriented
                switch (val) {
                    case float a when (a <= 33.3f): // Move speed +
                        return 2;
                    case float a when (a <= 66.6f): // Defence +
                        return 3;
                    case float a when (a <= 100f): // Max health +
                        return 4;
                }
                break;
            case 2: // 3rd upgrade - util oriented
                switch (val) {
                    case float a when (a <= 50f): // Exp +
                        return 5;
                    case float a when (a <= 100f): // Gold +
                        return 6;
                }
                break;
        }

        return -1;
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
