using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    /*
     * Timers
     * Minimap
     * StatusBars
     * Resources
     * UpgradeOptions
     */

    void Awake() {
        ReferanceManager.uiManager = this;
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

    #endregion

    #region Minimap

    #endregion
}
