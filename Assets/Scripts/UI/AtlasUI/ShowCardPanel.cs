using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowCardPanel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Instantiate(Resources.Load("Prefabs/UI/CardListPanel") as GameObject, GameObject.Find("Canvas").transform);
        
    }
}
