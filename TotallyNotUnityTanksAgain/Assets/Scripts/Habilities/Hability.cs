using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hability
{
    [HideInInspector]public bool isActive = false;
    [HideInInspector]public float initialTime = -1;
   
    public void Active()
    {
        if (!isActive)
        {
            initialTime = Time.time;
            isActive = true;
        }
    }

    public void Deactive()
    {
        if (isActive)
        {
            isActive = false;
        }
    }
}