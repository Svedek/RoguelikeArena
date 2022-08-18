using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Dropdown resolutionDropdown;

    [SerializeField] GameObject[] standaloneExclusiveOptions;
    void Awake() {
        #if UNITY_STANDALONE
        foreach (GameObject option in standaloneExclusiveOptions) {
            option.SetActive(true);
        }
        #endif

#if UNITY_WEBGL
        foreach (GameObject option in standaloneExclusiveOptions) {
            option.SetActive(false);
        }
#endif
    }

    private Resolution[] resolutions;
    private void Start() {
        // TODO load data if stored in file / cookies

        // TODO set buttons to the value of those settings

        #if UNITY_STANDALONE

        resolutions = Screen.resolutions;

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        //Resolution windowSize = new Resolution(Screen.width, Screen.height);
        for (int i = 0; i < resolutions.Length; i++) {
            string res = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "hz";
            resolutionOptions.Add(res);

            if (resolutions[i].Equals(Screen.currentResolution)) currentResolutionIndex = i;
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        #endif
    }

    public void SetVolumeMaster(float volume) => audioMixer.SetFloat("MasterVolume", volume);
    

    public void SetVolumeGame(float volume) => audioMixer.SetFloat("GameVolume", volume);

    public void SetVolumeMusic(float volume) => audioMixer.SetFloat("MusicVolume", volume);

    public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;

    public void SetResolution(int resIndex) {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    [SerializeField] Text guiScaleText;
    int guiScaleIndex = 2;
    private readonly float[] guiScales = {
        0.5f, 0.75f, 1f, 1.25f, 1.5f, 2f
    };
    public void ChangeGUIScale() {
        guiScaleIndex = (guiScaleIndex + 1) % guiScales.Length;
        guiScaleText.text = "GUI Scale: " + guiScales[guiScaleIndex];
        // TODO change guiscale to guiScales[guiScaleIndex];
    }
}
