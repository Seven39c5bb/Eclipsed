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
        currNodeID = SaveManager.instance.jsonData.mapData.currNodeID;
        this.gameObject.transform.position = MapManager.Instance.mapNodes[currNodeID.x][currNodeID.y].transform.position + new Vector3(0, 0.2f, 0);
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 mouseDelta = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
            float dampingFactor = 0.3f; // 阻尼系数，你可以根据需要调整
            this.gameObject.transform.Translate(mouseDelta * dampingFactor);
        }
        /* if (this.gameObject.transform.position != VirtualCamera.transform.position)
        {
            this.gameObject.transform.position = VirtualCamera.transform.position;
        } */
    }
}
