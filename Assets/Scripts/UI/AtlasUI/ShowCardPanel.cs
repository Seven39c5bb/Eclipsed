using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ShowCardPanel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject cardListPanel=Instantiate(Resources.Load("Prefabs/UI/CardListPanel"), GameObject.Find("Canvas").transform) as GameObject;
        cardListPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 500);
        cardListPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
    }
    public void ClosePanel()
    {
        GameObject cardListPanel=GameObject.Find("CardListPanel(Clone)");
        cardListPanel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 500), 0.5f).OnComplete(() =>
        {
            Destroy(cardListPanel);
        });
        
    }
}
