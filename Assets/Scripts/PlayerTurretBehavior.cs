using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTurretBehavior : MonoBehaviour
{
    public float rateOfFire = 0.25f;
    public float projectileX = 0;
    public float projectileY = 0;

    private float _cooldownTimer = 0;
    private PlayerUpgradeManager _upgradeManager;
    private GameEventHandler _eventHandler;

    // Start is called before the first frame update
    void Awake()
    {
        // Find projectile from ShipBehavior
        _upgradeManager = FindObjectOfType<PlayerUpgradeManager>();
        _eventHandler = FindObjectOfType<GameEventHandler>();
    }


    // Update is called once per frame
    void Update()
    {
        _cooldownTimer -= Time.deltaTime;

        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetButton("Fire1") && _cooldownTimer <= 0)
        {
            _cooldownTimer = rateOfFire;
            Fire();
        }

    }

    void Fire()
    {
        Vector3 offset = transform.rotation * new Vector3(projectileX, projectileY, 0);
        Instantiate(_upgradeManager.activeProjectile, transform.position + offset, transform.rotation);
        _eventHandler.playerWeapon.Play();
    }
}
