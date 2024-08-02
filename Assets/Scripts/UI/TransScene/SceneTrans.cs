using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrans : MonoBehaviour
{
    public Animator transition;
    public static SceneTrans sce_instance;
    public static SceneTrans instance
    {
        get
        {
            if(sce_instance == null)
            {
                sce_instance = FindObjectOfType<SceneTrans>();
            }
            return sce_instance;
        }
    }
    public void LoadNextScene_Trans(string sceneName)
    {
        StartCoroutine(LoadNextScene(sceneName));
    }
    IEnumerator LoadNextScene(string sceneName)
    {
        transition.SetTrigger("start");
        yield return new WaitForSeconds(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
