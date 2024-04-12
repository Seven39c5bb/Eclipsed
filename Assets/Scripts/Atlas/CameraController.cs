using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject VirtualCamera;
    private Vector2Int currNodeID;
    void Start()
    {
        VirtualCamera = GameObject.Find("Virtual Camera");
        if (SaveManager.instance.jsonData.mapData.currNodeID != null)
        {
            currNodeID = SaveManager.instance.jsonData.mapData.currNodeID;
        }
        this.gameObject.transform.position = MapManager.Instance.mapNodes[currNodeID.x][currNodeID.y].transform.position + new Vector3(0.2f, 0, 0);
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Texture2D cursorTex = Resources.Load("Cursors/Cursors 256/Cursor_Hand") as Texture2D;
            Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);

            Vector3 mouseDelta = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
            float dampingFactor = 0.3f; // 阻尼系数，你可以根据需要调整
            this.gameObject.transform.Translate(mouseDelta * dampingFactor);
        }
        else
        {
            Texture2D cursorTex = Resources.Load("Cursors/Cursors 256/Cursor_Basic2") as Texture2D;
            Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);
        }
        
    }
}
