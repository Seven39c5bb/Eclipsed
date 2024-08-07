using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : CellProperty
{

    private void Awake()
    {
        this.propertyName = "Smoke";
        this.description = "遮挡物块";
    }
    public override void OnPlayerEnter()
    {
        Debug.Log("Enter in the Smoke");
    }
}
