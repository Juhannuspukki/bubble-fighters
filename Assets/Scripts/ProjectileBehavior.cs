using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float maxSpeed = 15f;
    public int damage = 1;
    
    private float _selfDestructTimer = 8f;
    private GenericFunctions _genericFunctions;

    private void Awake()
    {
        _genericFunctions = FindObjectOfType<GenericFunctions>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        Transform trans = transform;
        
        // Move forward
        trans.position += trans.right * (Time.deltaTime * maxSpeed);
        
        // Self destruct after n seconds
        _selfDestructTimer -= Time.deltaTime;
        
        // Also self destruct if too far from the bubble center
        Vector3 closestCenter = _genericFunctions.GetClosestCircle(trans.position);

        if (_selfDestructTimer <= 0 || Vector3.Distance(closestCenter, trans.position) > 13.25f)
        {
            Destroy(gameObject);
        }
    }
}
