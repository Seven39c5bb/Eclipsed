using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    //public SpriteRenderer Renderer;
    private void Start()
    {
        //Renderer = GetComponent<SpriteRenderer>();
    }
    public bool cureButton = false;
    private void Update()
    {
        if(cureButton)
        {
            PlayerController.instance.Cure(20);
            cureButton = false;
        }
    }
    private void OnMouseEnter()
    {
        //Renderer.color = Color.red;
    }
    private void OnMouseExit()
    {
        //Renderer.color = Color.white;
    }

    public void Try()
    {
        Debug.Log("Try");
    }

}
