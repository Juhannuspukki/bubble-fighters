using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBehavior : MonoBehaviour
{
    private Camera _cam;
    private Rigidbody2D _rb;
    private GenericFunctions _genericFunctions;
    
    public Vector3 shipLocation;
    public bool shipIsWithinEngagementRange;
    
    public float movementForce;

    void Awake()
    {
        _genericFunctions = FindObjectOfType<GenericFunctions>();
        
        _rb = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
        
    }

    private void Update()
    {
        Vector3 shipPosition = transform.position;
        
        shipLocation = _genericFunctions.GetClosestCircle(shipPosition);
        shipIsWithinEngagementRange = Vector3.Distance(shipLocation, shipPosition) < 14;
                                      
        // Rotate
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 perpendicular = Vector3.Cross(shipPosition-mousePos,Vector3.forward);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
        
        // Move
        Vector3 impulse = new Vector3(Input.GetAxis("Horizontal") * movementForce, Input.GetAxis("Vertical") * movementForce, 0);
        _rb.AddForce(impulse, ForceMode2D.Impulse);

        if (Vector3.Distance(shipLocation, shipPosition) > 12f )
        { 
            // Add force towards the center on edges
            Vector3 direction = shipLocation - shipPosition;
            _rb.AddForce(direction * (movementForce/15.5f), ForceMode2D.Impulse);
        }

    }
}
