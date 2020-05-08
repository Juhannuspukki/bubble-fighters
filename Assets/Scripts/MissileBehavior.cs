using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    public int damage = 5;
    public float movementForce = 0.01f;

    private float _selfDestructTimer = 20f;
    private PlayerShipBehavior _ship;
    private Rigidbody2D _rb;
    private float _cooldownTimer = 0.1f;

    private void Start()
    {
        _ship = FindObjectOfType<PlayerShipBehavior>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (_ship.closestEnemy == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 position = transform.position;
        
        _cooldownTimer -= Time.deltaTime;
        
        if (_cooldownTimer <= 0)
        {
            // Rotate missile towards closest enemy
            Vector3 target = _ship.closestEnemy.transform.position;
            float angle = Mathf.Atan2(target.y-position.y, target.x-position.x)*180 / Mathf.PI;
            Vector3 rotation = new Vector3(0, 0, angle);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * 4f);
        }

        // Move
        Vector3 impulse = new Vector3(movementForce, 0, 0);
        _rb.AddRelativeForce(impulse, ForceMode2D.Impulse);

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
