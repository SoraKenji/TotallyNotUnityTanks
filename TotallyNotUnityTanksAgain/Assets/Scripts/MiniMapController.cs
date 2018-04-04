using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour {

    public Transform player;
    Vector3 newPosition;

    private void LateUpdate()
    {
        newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}