using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideButton : MonoBehaviour
{
    public void show()
    {
        if(GameObject.Find("GuidePanel(Clone)") == null)
        {
            Instantiate(Resources.Load("Prefabs/UI/GuidePanel"),GameObject.Find("Canvas").transform);
        }
        else
        {
            GameObject.Find("GuidePanel(Clone)").SetActive(true);
        }
    }
}
