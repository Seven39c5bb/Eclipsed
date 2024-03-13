using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    static void blockUI(string UIName)
    {
        GameObject obj=GameObject.Find(UIName);
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
    static void showUI(string UIName)
    {
        GameObject obj = GameObject.Find(UIName);
        if (obj != null) {
            obj.SetActive(true);
        }
    }
}
