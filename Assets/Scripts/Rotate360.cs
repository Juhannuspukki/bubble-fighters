using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate360 : MonoBehaviour
{
    private float _angle = 360.0f; // Degree per time unit
    private float _time = 3.0f; // Time unit in sec
    private Vector3 _axis = new Vector3(0, 0, 1); // Rotation axis, here it the yaw axis
    
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, _axis, _angle * Time.deltaTime / _time);
    }
}
