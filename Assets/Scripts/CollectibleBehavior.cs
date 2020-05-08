using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    public bool defaultCollectible = true;

    private GameEventHandler _eventHandler;
    private GameObject _playerObject;
    private GenericFunctions _genericFunctions;
    private Rigidbody2D _rb;
    private Vector3 _parentBubbleCenter;
    
    private void Awake()
    {
        _eventHandler = FindObjectOfType<GameEventHandler>();
        _playerObject = GameObject.FindWithTag("Player");
        _genericFunctions = FindObjectOfType<GenericFunctions>();
        _rb = GetComponent<Rigidbody2D>();
            
        _parentBubbleCenter = _genericFunctions.GetClosestCircle(transform.position);
    }
    
    private void Start()
    {
        // Move to random direction after spawning
        Vector3 impulse = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f), 0);
        _rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        // Move towards the player
        Vector3 position = transform.position;

        if (Vector3.Distance(position, _playerObject.transform.position) < 2f)
        {
            position = Vector2.MoveTowards(position, _playerObject.transform.position, 10f * Time.deltaTime);
            transform.position = position;
        }

        // Destroy if player wanders too far
        if (Vector3.Distance(_parentBubbleCenter, _playerObject.transform.position) > 13f)
        {
            Destroy(gameObject);
        }
        
        // Add force towards the center of parent bubble on edges
        if (Vector3.Distance(_parentBubbleCenter, position) > 11.5f )
        { 
            Vector3 direction = _parentBubbleCenter - position;
            _rb.AddForce(direction * 0.1f, ForceMode2D.Impulse);
        }
    }

    // Collect items and increase score
    private void OnTriggerEnter2D(Collider2D hit)
    {
        // Ignore hits by non-players
        if (!hit.CompareTag("Player")) return;
        
        Destroy(gameObject);
        
        // If this collectible is of default type, add a bubble
        if (defaultCollectible)
        {
            _eventHandler.AddPoint();
        }
        else
        {
            _eventHandler.AddPoint();
            _eventHandler.AddPoint();
            _eventHandler.AddPoint();
            _eventHandler.AddPoint();
            _eventHandler.AddPoint();
        }
    }
}
