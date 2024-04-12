using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject VirtualCamera;
    private Vector2Int currNodeID;
    private Texture2D cursorHand;
    private Texture2D cursorDefault;
    void Start()
    {
        VirtualCamera = GameObject.Find("Virtual Camera");
        cursorHand = Resources.Load("Cursors/Cursors 256/Cursor_Hand") as Texture2D;
        cursorDefault = Resources.Load("Cursors/Cursors 256/Cursor_Basic2") as Texture2D;
        if (SaveManager.instance.jsonData.mapData.currNodeID != null)
        {
            currNodeID = SaveManager.instance.jsonData.mapData.currNodeID;
            this.gameObject.transform.position = MapManager.Instance.mapNodes[currNodeID.x][currNodeID.y].transform.position + new Vector3(0.2f, 0, 0);
        }
    }
    private bool cursorIsHand = false;
    void Update()
    {
        
        if (Input.GetMouseButton(1))
        {
            Cursor.SetCursor(cursorHand, Vector2.zero, CursorMode.Auto);

            Vector3 mouseDelta = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
            float dampingFactor = 0.3f; // 阻尼系数，你可以根据需要调整
            this.gameObject.transform.Translate(mouseDelta * dampingFactor);
            cursorIsHand = true;
        }
        else if(cursorIsHand)
        {
            Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
            cursorIsHand = false;
        }
    }
}
