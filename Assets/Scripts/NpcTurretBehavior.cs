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
    private GameObject _playerObject;

    private void Awake()
    {
        _playerObject = GameObject.FindWithTag("Player");
        
        float closestCenterX = Convert.ToSingle(Math.Floor((transform.position.x + 12.5) / 25) * 25f);
        float closestCenterY = Convert.ToSingle(Math.Floor((transform.position.y + 12.5) / 25) * 25f);

        Vector3 closestCenter = new Vector3(closestCenterX, closestCenterY, 0);
        
        _parentCenter = closestCenter;
    }

    // Update is called once per frame
    void Update()
    {
        _cooldownTimer -= Time.deltaTime;

        if (_cooldownTimer <= 0 && Vector3.Distance(_parentCenter, _playerObject.transform.position) < 12f)
        {
            _cooldownTimer = fireDelay;
            Fire();
        }
    }
    
    void Fire()
    {
        Vector3 offset = transform.rotation * new Vector3(0.5f, 0, 0);
        Instantiate(projectile, transform.position + offset, transform.rotation);
    }
}
