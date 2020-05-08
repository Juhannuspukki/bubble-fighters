using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NpcTurretBehavior : MonoBehaviour
{
    public GameObject projectile;
    public float fireDelay = 1f;
    
    private float _cooldownTimer = 2f;
    private Vector3 _parentCenter;
    private PlayerShipBehavior _playerShip;

    private void Awake()
    {
        // Randomize delay before firing
        _cooldownTimer = UnityEngine.Random.Range(2.5f, 1.0f); 
        _playerShip = FindObjectOfType<PlayerShipBehavior>();
        _parentCenter = FindObjectOfType<GenericFunctions>().GetClosestCircle(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // If in different bubble or out of range do nothing
        if (_playerShip.shipLocation != _parentCenter) return;
        if (!_playerShip.shipIsWithinEngagementRange) return;

        _cooldownTimer -= Time.deltaTime;

        // If cooldowntimer has not expired yet, do nothing
        if (_cooldownTimer > 0) return;
        
        // Else fire weapon
        _cooldownTimer = fireDelay;
        Fire();
    }
    
    void Fire()
    {
        Vector3 offset = transform.rotation * new Vector3(0.5f, 0, 0);
        Instantiate(projectile, transform.position + offset, transform.rotation);
    }
}
