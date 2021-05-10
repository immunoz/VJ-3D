using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offset;


    void Update()
    {
        //transform.position = target.transform.position;
        Vector3 temp = new Vector3(target.position.x, target.position.y + offset, target.position.z - offset);
        transform.position = temp;
    }
}
