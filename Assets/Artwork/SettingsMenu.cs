using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.IO;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Sprite slider_activated;
    [SerializeField] private Sprite slider_deactivated;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundeffectSlider;

    [SerializeField] private Image cameraShakeSlider;

    [SerializeField] private GameObject menu;
    private bool cameraShakeEnabled;

    [SerializeField] private string settingsSavesFileName = "/Settings.json";
    [SerializeField] private string dataPath;

    public static SettingsSave settings = new SettingsSave();

    public static string settingsSaveFilePath;
    //public string settingFilePath { get { return settingsSavesFileName} set; }
    public void SetMenu(bool active)
    {
        //if (active) AudioManager.instance.Play("Button Click");
        Debug.Log("agf");
        if (active)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            animator.SetTrigger("Close");
        }
    }
    public void PlayCloseAnimation()
    {
        AudioManager.instance.Play("Button Click");
        animator.SetTrigger("Close");
    }
    public void ChangeMusicVolume(float volume)
    {
        settings.musicVolume = volume;
        audioMixer.SetFloat("musicVolume", volume);

        SaveSettingsFile();
    }
    public void ChangeSoundeffectVolume(float volume)
    {
        settings.sfxVolume = volume;
        audioMixer.SetFloat("soundeffectsVolume", volume);

        SaveSettingsFile();
    }
    public void ChangeCameraShakeState()
    {
        cameraShakeEnabled = !cameraShakeEnabled;

        settings.enabledCameraShake = cameraShakeEnabled;

        cameraShakeSlider.sprite = cameraShakeEnabled ? slider_activated : slider_deactivated;

        SaveSettingsFile();
    }


    public void CreateDirectory()
    {
        dataPath = Application.dataPath + "/SaveFiles/";
        Directory.CreateDirectory(dataPath);

        settingsSaveFilePath = dataPath + settingsSavesFileName;

        if (File.Exists(dataPath + settingsSavesFileName)) return;

        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(dataPath + settingsSavesFileName, json);
    }
    public void SaveSettingsFile()
    {
        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(dataPath + settingsSavesFileName, json);

        settingsSaveFilePath = dataPath + settingsSavesFileName;
    }
    public void ReadSettingsFile()
    {
        settings = GetData();
    }
    private void Awake()
    {
        // Gets the path of the folder with Save Files and creates a fodler if there are none

        CreateDirectory();

        // Then reads the data from the save File

        ReadSettingsFile();

       // SetMenu(false);
        musicSlider.value = settings.musicVolume;
        soundeffectSlider.value = settings.sfxVolume;


        cameraShakeEnabled = settings.enabledCameraShake;


        cameraShakeSlider.sprite = cameraShakeEnabled ? slider_activated : slider_deactivated;

        //animator = menu.GetComponent<Animator>();
    }
    private void Start()
    {
        Debug.Log(GetData().musicVolume);

        Debug.Log(audioMixer.SetFloat("musicVolume", GetData().musicVolume));
        audioMixer.SetFloat("soundeffectsVolume", GetData().sfxVolume);
    }
    public static SettingsSave GetData()
    {
        if (string.IsNullOrEmpty(settingsSaveFilePath)) settingsSaveFilePath = Application.dataPath + "/SaveFiles/" + "Settings.json";
        Debug.Log(settingsSaveFilePath);
        return JsonUtility.FromJson<SettingsSave>(File.ReadAllText(settingsSaveFilePath));
    }
}

[System.Serializable]
public class SettingsSave
{
    public bool enabledCameraShake = true;
    public bool immortal = false;

    public float sfxVolume;
    public float musicVolume;
}
