using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    //��Ʒ����
    public string itemName;
    //��ʲô����
    public enum buyItem { coin,fingerbone}
    public enum itemType { card,deleteCard}
    public itemType type;
    public buyItem buyType;
    //��Ʒ�۸�
    public int price;
    //��Ʒͼ��
    public Sprite itemSprite;
    //��Ʒ����
    public string itemDescription;
    //��Ʒ�ɹ������
    public int buyTimes;
    //��������������0�������ٴι���
    //����
    void BUY_THIS()
    {
        //�����������Ʒʱ
        switch(this.type)
        {
            case itemType.card:
                //������
                //������Ŀ��Ʒ��뿨��
                BuyCard();
                break;
            case itemType.deleteCard:
                //����ɾ������
                //��ɾ�����ƵĽ���
                //ɾ������
                break;
        }
    }
    public void BuyCard()
    {
        //�ı���Ʒ��Ʒ��ɫΪ��ɫ��������ѹ���
        //��Ǯ
        if (this.buyType == buyItem.coin)
        {
            if (SaveManager.instance.jsonData.playerData.coin >= price)
            {
                SaveManager.instance.jsonData.playerData.coin-=price;
            }
            else { Debug.Log("��Ҳ���"); }/*��ʾ��Ҳ���*/
        }
        else
        {
            if (SaveManager.instance.jsonData.playerData.fingerBone >= price)
            { SaveManager.instance.jsonData.playerData.fingerBone -= price; }
            else { }/*��ʾ��ͷ����*/
        }
        //�����Ʒ��뿨��
        SaveManager.instance.jsonData.playerData.playerDeck.Add(this.name);
        SaveManager.instance.Save();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //��ʾ�Ƿ���
        Debug.Log("buy card");
        BuyCard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
    }
}
