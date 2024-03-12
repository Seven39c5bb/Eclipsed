using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    public SpriteRenderer Renderer;
    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }
    private void OnMouseEnter()
    {
        Renderer.color = Color.red;
    }
    private void OnMouseExit()
    {
        Renderer.color = Color.white;
    }

}
