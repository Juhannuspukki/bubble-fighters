using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBehavior : MonoBehaviour
{
    private Camera _cam;
    private Rigidbody2D _rb;
    private GenericFunctions _genericFunctions;
    private GenerateMap _generateMap;
    private Architect _architect;

    public Vector3 previousShipLocation;
    public Vector3 shipLocation;
    public bool shipIsWithinEngagementRange;
    
    public float movementForce;

    void Awake()
    {
        _genericFunctions = FindObjectOfType<GenericFunctions>();
        _generateMap = FindObjectOfType<GenerateMap>();
        _architect = FindObjectOfType<Architect>();

        _rb = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
        
    }

    private void Update()
    {
        Vector3 shipPosition = transform.position;
        
        Vector3 newShipLocation = _genericFunctions.GetClosestCircle(shipPosition);
        shipIsWithinEngagementRange = Vector3.Distance(shipLocation, shipPosition) < 13.5;

        // Only update shipLocation if it has changed
        if (newShipLocation != previousShipLocation)
        {
            previousShipLocation = shipLocation;
            shipLocation = newShipLocation;
            
            _architect.GenerateWorld(shipLocation);
            _generateMap.UpdateMap(shipLocation);
        }
                                      
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
