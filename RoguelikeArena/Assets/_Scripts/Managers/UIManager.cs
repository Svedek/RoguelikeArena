using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance { get; private set; }



    /*
     * Timers
     * Minimap
     * StatusBars
     * Resources
     * UpgradeOptions
     */

    void Awake() {
        // Singleton Setup
        if (Instance != null) {
            Debug.LogError("Multiple instances of UIManager!");
        }
        Instance = this;

        // Initialize UIManager
        SetDefaultValues();
        HideUpgradeOptions();
    }

    /*
    [SerializeField] float scale;
    public void Update() {
        ResizeGUI(scale);
    }
    */

    #region GUI Main
    [SerializeField] GameObject[] uiFolders;
    public void ResizeGUI(float scale) {
        for(int i = 0; i<uiFolders.Length; i++) {
            uiFolders[i].transform.localScale = Vector3.one*scale;
        }
    }

    public void SetDefaultValues() {
        healthSlider.value = healthSlider.maxValue; // When it is changed it will also change the max value
        expSlider.value = expSlider.minValue;
        levelText.text = "1";

        goldText.text = resource2Text.text = "0";
    }
    #endregion

    #region Status Bars
    [Header("Status Bars")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Text levelText;

    public void UpdateHealth(float currentHealth, float maxHealth) {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void UpdateExperience(float currentExperience, float maxExperience, int level) {
        expSlider.maxValue = maxExperience;
        expSlider.value = currentExperience;
        levelText.text = "" + level;
    }
    #endregion

    #region Resources
    [Header("Resources")] // Maybe add fade in of gained resources and fade out
    [SerializeField] private Text goldText;
    [SerializeField] private Text resource2Text;
    public void UpdateGold(int val) {
        goldText.text = "" + val;
    }
    public void UpdateResource2(int val) {
        resource2Text.text = "" + val;
    }
    #endregion

    #region Upgrade Options
    [Header("Upgrade Options")]
    [SerializeField] GameObject[] UpgradeOptions;
    public void SetUpgradeOptions(Color[] color, string[] text) {
        for (int i = 0; i < UpgradeOptions.Length; i++) {
            GameObject current = UpgradeOptions[i];

            current.GetComponent<Image>().color = color[i];
            current.GetComponentInChildren<Text>().text = text[i];

            current.SetActive(true);
        }
    }

    public void HideUpgradeOptions() {
        for (int i = 0; i<UpgradeOptions.Length; i++) {
            UpgradeOptions[i].SetActive(false);
        }
    }
    #endregion

    #region Timers
    [Header("Status Bars")]
    [SerializeField] private Text globalTime;
    [SerializeField] private Text gameTime;

    public void SetTime(float global, float game) {
        int minutes = (int)global / 60;
        float seconds = global % 60;
        
        globalTime.text = minutes + ":" + seconds.ToString("n3");

        minutes = (int)game / 60;
        seconds = game % 60;
        gameTime.text = minutes + ":" + seconds;
    }
    #endregion

    #region Minimap
    [Header("Upgrade Options")]
    [SerializeField] Camera minimapCamera;
    private const int roomSize = 60; 
    public void UpdateMinimap(int roomBounds) {
        int pos = (roomBounds / 2) * roomSize;
        int size = (int) ((roomBounds / 2f) * roomSize);

        minimapCamera.transform.position = new Vector3(pos, pos, -10f);
        minimapCamera.orthographicSize = size;
    }
    #endregion
}
