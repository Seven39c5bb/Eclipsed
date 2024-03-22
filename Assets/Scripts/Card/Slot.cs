using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool isEmpty = true;
    public Slot instance;
    private void Awake()
    {
        instance = this;
    }
}
