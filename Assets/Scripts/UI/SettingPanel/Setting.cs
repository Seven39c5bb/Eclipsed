using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public GameObject settingPanel;
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}

