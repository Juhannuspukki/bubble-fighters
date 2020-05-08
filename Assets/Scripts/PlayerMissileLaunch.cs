using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileLaunch : MonoBehaviour
{
    public float rateOfFire = 5f;
    public float projectileX = 0;
    public float projectileY = 0;
    
    private float _cooldownTimer = 0;
    private PlayerShipBehavior _ship;
    private GameEventHandler _eventHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        _ship = FindObjectOfType<PlayerShipBehavior>();
        _eventHandler = FindObjectOfType<GameEventHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        _cooldownTimer -= Time.deltaTime;

        if (Input.GetButton("Fire2") && _cooldownTimer <= 0)
        {
            _cooldownTimer = rateOfFire;
            LaunchMissile();
        }
    }
    
    
    void LaunchMissile()
    {
        Vector3 offset = transform.rotation * new Vector3(projectileX, projectileY, 0);
        Instantiate(_ship.activeMissile, transform.position + offset, transform.rotation);
        _eventHandler.playerWeapon.Play();
    }
}
