using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    public int damage = 5;
    public float movementForce = 0.01f;
    public GameObject damageParticle;

    private Vector3 _closestCenter;
    private Rigidbody2D _rb;
    private GameObject _closestEnemy;
    
    private float _selfDestructTimer = 20f;
    private float _cooldownTimer = 0.1f;

    private void Start()
    {
        Vector3 position = transform.position;
        
        _closestCenter = GenericFunctions.GetClosestCircle(position);
        _closestEnemy = GenericFunctions.FindClosestEnemy(position);

        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 position = transform.position;
        
        // Find closest enemy, if no enemy is currently stored in _closestEnemy
        if (_closestEnemy == null)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            _closestEnemy = GenericFunctions.FindClosestEnemy(transform.position);
            
            // If _closestEnemy is still null, it means nothing new was found
            if (_closestEnemy == null)
            {
                Destroy(gameObject);
                return;
            }
        }
        
        _cooldownTimer -= Time.deltaTime;
        
        if (_cooldownTimer <= 0)
        {
            // Rotate missile towards closest enemy
            Vector3 target = _closestEnemy.transform.position;
            float angle = Mathf.Atan2(target.y-position.y, target.x-position.x)*180 / Mathf.PI;
            Vector3 rotation = new Vector3(0, 0, angle);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * 10f);
        }

        // Move forward
        Vector3 impulse = new Vector3(movementForce, 0, 0);
        _rb.AddRelativeForce(impulse, ForceMode2D.Impulse);

        // Self destruct after n seconds
        _selfDestructTimer -= Time.deltaTime;

        // Also self-destruct if hits the edge of the bubble
        if (_selfDestructTimer <= 0 || Vector3.Distance(_closestCenter, position) > 12f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
       SpawnDamageParticles(transform.position);
    }
    
    
    private void SpawnDamageParticles(Vector3 position)
    {
        int numberOfParticles = UnityEngine.Random.Range(5, 7);
        
        // Spawn collectible bubbles
        for (int i = 0; i < numberOfParticles; i++) 
        {
            Instantiate(damageParticle, position, transform.rotation);
        }
    }
}
