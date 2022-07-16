using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTarget : MonoBehaviour
{
    public Transform player;


    void Update()
    {
        transform.position = player.transform.position + Vector3.up;
    }
}
