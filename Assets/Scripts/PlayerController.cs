using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private Vector3 offset;
    
    private Rigidbody Rigidbody;
    public float speed;//测试移动
    public float inputX, inputY;
    // Start is called before the first frame update
    void Start()
    {
        offset = Camera.main.transform.position - transform.position;
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = UnityEngine.Input.GetAxisRaw("Horizontal");
        inputY = UnityEngine.Input.GetAxisRaw("Vertical");
        transform.position += new Vector3(inputX * speed * Time.deltaTime, inputY * speed * Time.deltaTime, 0);
        Camera.main.transform.position = offset + transform.position;
    }
}
