using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 newRotation = gameObject.transform.rotation.eulerAngles + new Vector3(0, Time.deltaTime, 0);
        Quaternion q = Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z);
        gameObject.transform.rotation.Set(q.x, q.y, q.z, q.w);
    }
}
