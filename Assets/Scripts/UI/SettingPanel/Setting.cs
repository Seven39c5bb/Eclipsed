using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public GameObject settingPanel;
    private Slider TotalAudioSlider;
    private Slider BGMSlider;
    private Slider SFXSlider;
    private AudioMixer audioMixer;
    void Awake()
    {
        TotalAudioSlider = settingPanel.transform.Find("Panel(Setting)/TotalAudioSlider").GetComponent<Slider>();
        BGMSlider = settingPanel.transform.Find("Panel(Setting)/BGMSlider").GetComponent<Slider>();
        SFXSlider = settingPanel.transform.Find("Panel(Setting)/SFXSlider").GetComponent<Slider>();
        audioMixer = Resources.Load<AudioMixer>("Audios/AudioMixer");
        float TotalVolume, BGMVolume, SFXVolume;
        audioMixer.GetFloat("Total", out TotalVolume);
        audioMixer.GetFloat("BGM", out BGMVolume);
        audioMixer.GetFloat("SFX", out SFXVolume);
        TotalAudioSlider.value = TotalVolume;
        BGMSlider.value = BGMVolume;
        SFXSlider.value = SFXVolume;

        TotalAudioSlider.onValueChanged.AddListener((value) =>
        {
            audioMixer.SetFloat("Total", value);
            if (value == TotalAudioSlider.minValue)
            {
                audioMixer.SetFloat("Total", -80);
            }
        });
        BGMSlider.onValueChanged.AddListener((value) =>
        {
            audioMixer.SetFloat("BGM", value);
            if (value == BGMSlider.minValue)
            {
                audioMixer.SetFloat("BGM", -80);
            }
        });
        SFXSlider.onValueChanged.AddListener((value) =>
        {
            audioMixer.SetFloat("SFX", value);
            if (value == SFXSlider.minValue)
            {
                audioMixer.SetFloat("SFX", -80);
            }
        });
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingPanel.SetActive(!settingPanel.activeSelf);
            if (settingPanel.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
    public void OnClickSetting()
    {
            settingPanel.SetActive(true);
            Time.timeScale = 0;
    }


    public void OnClickBackToGame()
    {
        settingPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnClickBackToMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}

