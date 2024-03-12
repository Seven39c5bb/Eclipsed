using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = Camera.main.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = offset + transform.position;
    }
}
