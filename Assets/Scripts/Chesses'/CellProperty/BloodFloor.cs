using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFloor : CellProperty
{
    void Awake()
    {
        this.propertyName = "BloodFloor";
        this.description = "染血的地面";
    }
}
