using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float maxSpeed = 15f;
    public int damage = 1;
    
    private float _selfDestructTimer = 8f;
    private Vector3 _closestCenter;
    
    
    private void Awake()
    {
        _closestCenter = GenericFunctions.GetClosestCircle(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Move forward
        transform.position += transform.right * (Time.deltaTime * maxSpeed);
        
        // Self destruct after n seconds
        _selfDestructTimer -= Time.deltaTime;

        // Also self destruct if too far from the bubble center
        if (_selfDestructTimer <= 0 || Vector3.Distance(_closestCenter, transform.position) > 13.25f)
        {
            Destroy(gameObject);
        }
    }
}
