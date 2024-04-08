using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager buff_instance;
    public static BuffManager instance
    {
        get
        {
            if (buff_instance == null)
            {
                buff_instance = GameObject.FindObjectOfType<BuffManager>();
            }
            return buff_instance;
        }
    }
}
