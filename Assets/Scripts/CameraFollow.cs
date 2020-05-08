using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject playerObject;
    
    private void Awake()
    {
        playerObject = GameObject.FindWithTag("Player");
    }
    
    void Update()
    {
        if (playerObject != null)
        {
            Vector3 position = playerObject.transform.position;
            position.z = transform.position.z;
            transform.position = position;

        }
    }
}
