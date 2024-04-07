using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Delete : MonoBehaviour,IPointerClickHandler
{
    public GameObject deletePanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        deletePanel.transform.DOMove(GameObject.Find("Canvas").transform.position, 0.5f);
    }
}
