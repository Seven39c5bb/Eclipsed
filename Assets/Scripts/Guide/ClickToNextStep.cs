using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToNextStep : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GuidePanel.instance.ExecuteNextStep(GuidePanel.instance.currentStep);
    }
}
