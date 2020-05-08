using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBehavior : MonoBehaviour
{
    private Camera _cam;
    private Rigidbody2D _rb;
    
    public float movementForce = 0.5f;
    public GameObject[] shipModels;
    public GameObject[] weaponModels;
    public GameObject[] projectiles;

    public string activeShipModel;
    public GameObject activeWeaponModel;
    public GameObject activeMissile;
    public GameObject activeProjectile;

    public GameObject closestEnemy;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cam = Camera.main;

        activeWeaponModel = weaponModels[0];
        activeProjectile = projectiles[0];
    }

    void FixedUpdate()
    {
        FindClosestEnemy();
            
        Vector3 shipPosition = transform.position;
        
        // Rotate
        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 perpendicular = Vector3.Cross(shipPosition-mousePos,Vector3.forward);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
        
        // Move
        Vector3 impulse = new Vector3(Input.GetAxis("Horizontal") * movementForce, Input.GetAxis("Vertical") * movementForce, 0);
        _rb.AddForce(impulse, ForceMode2D.Impulse);

        // Detemine where the center of the closest circle is
        float closestCenterX = Convert.ToSingle(Math.Floor((shipPosition.x + 12.5) / 25) * 25f);
        float closestCenterY = Convert.ToSingle(Math.Floor((shipPosition.y + 12.5) / 25) * 25f);

        Vector3 closestCenter = new Vector3(closestCenterX, closestCenterY, 0);
        
        
        if (Vector3.Distance(closestCenter, shipPosition) > 11.5f )
        { 
            // Add force towards the center on edges
            Vector3 direction = closestCenter - shipPosition;
            _rb.AddForce(direction * (movementForce/12), ForceMode2D.Impulse);
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
        movementForce = 0.2f;
        
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
                movementForce = 0.8f;
                hardPoints = new List<Vector3>{new Vector3(1.1f, 0, 0)} ;
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
        // Get the right gun
        GameObject newWeapon = weaponModels[0];
        
        switch (weaponId)
        {
            case "weapon_0":
                newWeapon = weaponModels[0];
                break;
            case "weapon_1":
                newWeapon = weaponModels[1];
                break;
            case "weapon_2":
                newWeapon = weaponModels[2];
                break;
            case "weapon_3":
                newWeapon = weaponModels[3];
                break;
        }

        // Set it as active
        activeWeaponModel = newWeapon;
        
        // Let UpdateShip handle the placing of new weapons
        UpdateShip(activeShipModel);
    }
    
    public void UpdateProjectile(string projectileId)
    {
        GameObject newProjectile = projectiles[0];
        
        switch (projectileId)
        {
            case "projectile_0":
                newProjectile = projectiles[0];
                break;
            case "projectile_1":
                newProjectile = projectiles[1];
                break;
            case "projectile_2":
                newProjectile = projectiles[2];
                break;
            case "projectile_3":
                newProjectile = projectiles[3];
                break;
        }

        activeProjectile = newProjectile;

    }
    
    private void FindClosestEnemy() 
    { 
        // Find all game objects with tag Enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            closestEnemy = null;
            return;
        }
        
        // Initialize with infinite distance
        float smallestDistance = Mathf.Infinity;
        Vector3 position = transform.position; 
   
        // Iterate through enemies and find the closest one
        foreach (GameObject enemy in enemies)
        { 
            Vector3 diff = (enemy.transform.position - position); 
            float currentDistance = diff.sqrMagnitude; 
            if (currentDistance < smallestDistance) 
            { 
                closestEnemy = enemy; 
                smallestDistance = currentDistance; 
            } 
        }
        
    }
}
