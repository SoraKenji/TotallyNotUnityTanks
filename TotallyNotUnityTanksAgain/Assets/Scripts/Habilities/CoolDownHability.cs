using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownHability : Hability
{
    public float coolDownTime;

    public bool Check()
    {
        if (coolDownTime + initialTime < Time.time && isActive)
        {
            coolDownTime = 0f;
            isActive = false;
            return true;
        }
        return false;
    }
}
