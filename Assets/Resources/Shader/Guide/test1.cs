using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    // Start is called before the first frame update
    public float dissolveTime = 3f;
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dissolve());
        }      
    }
    private IEnumerator Dissolve()
    {
        SetDissolveRate(1);
        float timeCounter = 0;
        while (timeCounter < dissolveTime)
        {
            timeCounter += Time.deltaTime;
            SetDissolveRate(1-timeCounter / dissolveTime);
            yield return null;
        }
    }
    public void SetDissolveRate(float value)
    {
        int shaderId= Shader.PropertyToID("_dissolveValue");
        spriteRenderer.material.SetFloat(shaderId, value);
    }
}
