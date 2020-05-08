using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    
    private GameEventHandler _eventHandler;
    public GameObject playerObject;
    public bool defaultCollectible = true;

    private void Awake()
    {
        _eventHandler = FindObjectOfType<GameEventHandler>();
        playerObject = GameObject.FindWithTag("Player");
    }
    
    private void Start()
    {
        // Move to random direction after spawning
        Vector3 impulse = new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), 0);
        GetComponent<Rigidbody2D>().AddForce(impulse, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        // Move towards the player
        Vector3 position = transform.position;
        position = Vector2.MoveTowards(position, playerObject.transform.position, 2f * Time.deltaTime);
        transform.position = position;
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
