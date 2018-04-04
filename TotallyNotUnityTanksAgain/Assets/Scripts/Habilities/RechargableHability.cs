using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RechargableHability : Hability {

    public float timeToRechargeAll;
    float partialRecharge = 0;
    float partialUsage = 0;


    public void SetRechargeHability()
    {
        partialUsage = (1/timeToRechargeAll) / Utils.getFramePerSeconds();
        partialRecharge = (1 / timeToRechargeAll/4) / Utils.getFramePerSeconds();
        Debug.Log(partialRecharge);
    }

    public float recharging(float slideValue)
    {
        if (slideValue < 1)
        {
            slideValue += partialRecharge;
            return slideValue;
        }
        else
        {
            return 1;
        }
    }

    public float usingHability (float slideValue)
    {
        if (slideValue > 0)
        {
            slideValue -= partialUsage;
            return slideValue;
        }
        else
        {
            slideValue = 0;
            isActive = false;
            return 0;
        }
    }
}
