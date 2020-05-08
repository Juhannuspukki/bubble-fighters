using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float maxSpeed = 15f;
    public int damage = 1;
    private float _selfDestructTimer = 8f;

    // Update is called once per frame
    void FixedUpdate()
    {
        
        // Move
        Vector3 position = transform.position;
        Vector3 velocity = new Vector3(maxSpeed * Time.deltaTime, 0, 0);
        position += transform.rotation * velocity;
        transform.position = position;
        
        // Self destruct after n seconds
        _selfDestructTimer -= Time.deltaTime;
        
        // Detemine where the center of the closest circle is
        float closestCenterX = Convert.ToSingle(Math.Floor((position.x + 12.5) / 25) * 25f);
        float closestCenterY = Convert.ToSingle(Math.Floor((position.y + 12.5) / 25) * 25f);

        Vector3 closestCenter = new Vector3(closestCenterX, closestCenterY, 0);

        if (_selfDestructTimer <= 0 || Vector3.Distance(closestCenter, position) > 12f)
        {
            Destroy(gameObject);
        }
    }
}
