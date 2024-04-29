using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Step : MonoBehaviour
{
    public int eventIndex;
    public RectTransform target;
    //TODO
    public void Execute()
    {
        this.gameObject.SetActive(true);
        target.AddComponent<ClickToNextStep>();
        RectGuide.instance.Rect_Guide(target);
    }
}
