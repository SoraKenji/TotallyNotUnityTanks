using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBoundaries : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        string[] recognizer = other.tag.Split('_');
        if(recognizer[0] == "Player")
        {
            PlayerMovement plMV = other.transform.parent.GetComponent<PlayerMovement>();
            Debug.Log("asdf");
            plMV.makeItFlip();
        }
    }
}
