using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

	public static float getFramePerSeconds()
    {
        return (1 / Time.deltaTime);
    }
}
