using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBehavior : MonoBehaviour
{
    private Camera _cam;
    private Rigidbody2D _rb;
    private GenericFunctions _genericFunctions;
    
    public float movementForce = 0.5f;
    public GameObject[] shipModels;
    public GameObject[] weaponModels;
    public GameObject[] projectiles;

    public string activeShipModel;
    public GameObject activeWeaponModel;
    public GameObject activeMissile;
    public GameObject activeProjectile;
    
    void Awake()
    {
        _genericFunctions = FindObjectOfType<GenericFunctions>();
        
        _rb = GetComponent<Rigidbody2D>();
        _cam = Camera.main;

        activeWeaponModel = weaponModels[0];
        activeProjectile = projectiles[0];
    }

    void FixedUpdate()
    {
        Vector3 shipPosition = transform.position;
        
        // Rotate
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 perpendicular = Vector3.Cross(shipPosition-mousePos,Vector3.forward);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
        
        // Move
        Vector3 impulse = new Vector3(Input.GetAxis("Horizontal") * movementForce, Input.GetAxis("Vertical") * movementForce, 0);
        _rb.AddForce(impulse, ForceMode2D.Impulse);
        
        Vector3 closestCenter = _genericFunctions.GetClosestCircle(shipPosition);

        if (Vector3.Distance(closestCenter, shipPosition) > 10.5f )
        { 
            // Add force towards the center on edges
            Vector3 direction = closestCenter - shipPosition;
            _rb.AddForce(direction * (movementForce/14), ForceMode2D.Impulse);
        }
        
        if (Vector3.Distance(closestCenter, shipPosition) > 12f )
        { 
            // Add force towards the center on edges
            Vector3 direction = closestCenter - shipPosition;
            _rb.AddForce(direction * (0.006f), ForceMode2D.Impulse);
        }

    }

    public void UpdateShip(string shipId)
    {
        // Destroy all ship and weapon models
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        List<Vector3> hardPoints = new List<Vector3>{Vector3.zero};
        GameObject newShip = shipModels[0];
        
        switch (shipId)
        {
            case "ship_0":
                newShip = shipModels[0];
                movementForce = 0.5f;
                hardPoints = new List<Vector3>{new Vector3(0, 0, 0)} ;
                break;
            case "ship_1":
                newShip = shipModels[1];
                movementForce = 0.6f;
                hardPoints = new List<Vector3>{new Vector3(-0.1f, 0, 0)} ;
                break;
            case "ship_2":
                newShip = shipModels[2];
                movementForce = 0.7f;
                hardPoints = new List<Vector3>{new Vector3(0.2f, 0, 0)} ;
                break;
            case "ship_3":
                newShip = shipModels[3];
                movementForce = 0.7f;
                hardPoints = new List<Vector3>{new Vector3(1.1f, 0, 0)} ;
                break;
            case "ship_4":
                newShip = shipModels[4];
                movementForce = 0.8f;
                hardPoints = new List<Vector3>{new Vector3(0, 0, 0)} ;
                break;
        }
        
        GameObject ship = Instantiate(newShip, transform.position , transform.rotation);
        ship.transform.parent = gameObject.transform;

        foreach (var hardpointLocation in hardPoints)
        {
            GameObject weapon = Instantiate(activeWeaponModel, transform.position, transform.rotation);
            weapon.transform.parent = gameObject.transform;
            weapon.transform.localPosition = hardpointLocation;
        }

        activeShipModel = shipId;
    }
    
    public void UpdateWeapon(string weaponId)
    {
        string[] splittedString = weaponId.Split('_');
        int modifier = Int32.Parse(splittedString[1]);
        
        // Set it as active
        activeWeaponModel = weaponModels[modifier];
        
        // Let UpdateShip handle the placing of new weapons
        UpdateShip(activeShipModel);
    }
    
    public void UpdateProjectile(string projectileId)
    {
        string[] splittedString = projectileId.Split('_');
        int modifier = Int32.Parse(splittedString[1]);
        
        activeProjectile = projectiles[modifier];

    }
}
